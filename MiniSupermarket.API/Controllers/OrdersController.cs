using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;
using System.Data;

namespace MiniSupermarket.API.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public OrdersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // POST: api/orders hoặc api/orders/checkout
        // Nghiệp vụ thanh toán POS sử dụng thuần SqlTransaction đảm bảo chuẩn ACID và Rollback khi hết hàng (TC35)
        [HttpPost]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var items = (request.Items != null && request.Items.Count > 0)
                ? request.Items
                : (request.Details ?? new List<CheckoutRequestItem>());

            if (items == null || items.Count == 0)
                return BadRequest(new { message = "Đơn hàng phải có ít nhất một sản phẩm." });

            if (items.Any(item => item.Quantity <= 0))
                return BadRequest(new { message = "Số lượng sản phẩm phải lớn hơn 0." });

            var connectionString = _context.Database.GetConnectionString();
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Khởi tạo Transaction trên SqlConnection thuần ADO.NET
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Xác định nhân viên thu ngân (Cashier)
                string username = (request.CashierUsername ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(username) && User.Identity?.IsAuthenticated == true)
                {
                    username = User.Identity.Name ?? string.Empty;
                }

                int? cashierId = null;
                using (var cashierCmd = connection.CreateCommand())
                {
                    cashierCmd.Transaction = transaction;
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        cashierCmd.CommandText = "SELECT TOP 1 UserId FROM Users WHERE LOWER(Username) = LOWER(@Username)";
                        cashierCmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username });
                        var result = await cashierCmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            cashierId = Convert.ToInt32(result);
                        }
                    }

                    if (!cashierId.HasValue)
                    {
                        cashierCmd.Parameters.Clear();
                        cashierCmd.CommandText = "SELECT TOP 1 UserId FROM Users ORDER BY UserId";
                        var result = await cashierCmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            cashierId = Convert.ToInt32(result);
                        }
                    }

                }

                if (!cashierId.HasValue)
                {
                    throw new InvalidOperationException("Không xác định được tài khoản thu ngân thực hiện giao dịch.");
                }

                // 2. Xác định thông tin khách hàng thân thiết (nếu có)
                int? customerId = null;
                string? customerName = null;
                int currentRewardPoints = 0;
                string currentMembershipRank = "Đồng";

                string phone = (request.CustomerPhone ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    using var customerCmd = connection.CreateCommand();
                    customerCmd.Transaction = transaction;
                    customerCmd.CommandText = @"
                        SELECT TOP 1 CustomerId, FullName, ISNULL(RewardPoints, 0) AS RewardPoints, ISNULL(MembershipRank, N'Đồng') AS MembershipRank 
                        FROM Customers 
                        WHERE PhoneNumber = @Phone OR LOWER(FullName) = LOWER(@Phone)";
                    customerCmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 50) { Value = phone });

                    using var custReader = await customerCmd.ExecuteReaderAsync();
                    if (await custReader.ReadAsync())
                    {
                        customerId = custReader.GetInt32(custReader.GetOrdinal("CustomerId"));
                        customerName = custReader.GetString(custReader.GetOrdinal("FullName"));
                        currentRewardPoints = custReader.GetInt32(custReader.GetOrdinal("RewardPoints"));
                        currentMembershipRank = custReader.GetString(custReader.GetOrdinal("MembershipRank"));
                    }
                }

                int redeemPoints = request.RedeemPoints;
                if (redeemPoints < 0)
                {
                    return BadRequest(new { message = "Số điểm sử dụng không được âm." });
                }

                if (redeemPoints > 0 && !customerId.HasValue)
                {
                    return BadRequest(new { message = "Chỉ khách hàng thành viên mới được sử dụng điểm." });
                }

                if (redeemPoints > currentRewardPoints)
                {
                    return BadRequest(new { message = "Số điểm sử dụng vượt quá điểm hiện có." });
                }

                // Gom nhóm các sản phẩm trùng mã để tính tổng số lượng đặt mua
                var groupedItems = items
                    .GroupBy(i => i.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    })
                    .ToList();

                // 3. Kiểm tra tính hợp lệ và tồn kho từng mặt hàng (Khóa dòng UPDLOCK chống Race Condition)
                decimal totalAmount = 0m;
                var orderDetails = new List<(int ProductId, int Quantity, decimal UnitPrice)>();

                foreach (var item in groupedItems)
                {
                    using var stockCmd = connection.CreateCommand();
                    stockCmd.Transaction = transaction;
                    stockCmd.CommandText = @"
                        SELECT TOP 1 ProductName, Price, StockQuantity 
                        FROM Products WITH (UPDLOCK) 
                        WHERE ProductId = @ProductId";
                    stockCmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = item.ProductId });

                    using var prodReader = await stockCmd.ExecuteReaderAsync();
                    if (!await prodReader.ReadAsync())
                    {
                        throw new InvalidOperationException($"Sản phẩm có mã ID = {item.ProductId} không còn tồn tại trong hệ thống!");
                    }

                    string prodName = prodReader.GetString(prodReader.GetOrdinal("ProductName"));
                    decimal unitPrice = prodReader.GetDecimal(prodReader.GetOrdinal("Price"));
                    int currentStock = prodReader.GetInt32(prodReader.GetOrdinal("StockQuantity"));

                    // Kiểm tra tồn kho: Nếu số lượng đặt lớn hơn tồn kho thực tế -> Ném ngoại lệ kích hoạt Rollback (TC35)
                    if (currentStock < item.Quantity)
                    {
                        throw new InvalidOperationException(
                            $"Sản phẩm '{prodName}' không đủ tồn kho để đáp ứng! Tồn kho hiện tại: {currentStock}, yêu cầu: {item.Quantity}.");
                    }

                    totalAmount += unitPrice * item.Quantity;
                    orderDetails.Add((item.ProductId, item.Quantity, unitPrice));
                }

                decimal discountAmount = redeemPoints * 1000m;
                if (discountAmount > totalAmount)
                {
                    return BadRequest(new { message = "Số điểm sử dụng không được vượt quá giá trị hóa đơn." });
                }

                decimal finalAmount = totalAmount - discountAmount;

                // 4. Sinh bản ghi hóa đơn trong bảng Orders
                int newOrderId;
                using (var orderCmd = connection.CreateCommand())
                {
                    orderCmd.Transaction = transaction;
                    orderCmd.CommandText = @"
                        INSERT INTO Orders (OrderDate, TotalAmount, Status, CustomerId, UserId)
                        OUTPUT INSERTED.OrderId
                        VALUES (@OrderDate, @TotalAmount, @Status, @CustomerId, @UserId)";

                    orderCmd.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.DateTime2) { Value = DateTime.Now });
                    orderCmd.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = finalAmount, Precision = 18, Scale = 2 });
                    orderCmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 50) { Value = "Completed" });
                    orderCmd.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Int) { Value = (object?)customerId ?? DBNull.Value });
                    orderCmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = cashierId.Value });

                    newOrderId = Convert.ToInt32(await orderCmd.ExecuteScalarAsync());
                }

                // 5. Thêm từng dòng chi tiết vào bảng OrderItems và trừ số lượng tồn kho trong bảng Products
                foreach (var detail in orderDetails)
                {
                    using var itemCmd = connection.CreateCommand();
                    itemCmd.Transaction = transaction;
                    itemCmd.CommandText = @"
                        INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
                        VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);

                        UPDATE Products
                        SET StockQuantity = StockQuantity - @Quantity
                        WHERE ProductId = @ProductId;";

                    itemCmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = newOrderId });
                    itemCmd.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = detail.ProductId });
                    itemCmd.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int) { Value = detail.Quantity });
                    itemCmd.Parameters.Add(new SqlParameter("@UnitPrice", SqlDbType.Decimal) { Value = detail.UnitPrice, Precision = 18, Scale = 2 });

                    await itemCmd.ExecuteNonQueryAsync();
                }

                // 6. Tích lũy điểm thưởng thành viên trong bảng Customers (1 điểm mỗi 10.000 VNĐ)
                if (customerId.HasValue)
                {
                    int earnedPoints = (int)(finalAmount / 10000);
                    if (earnedPoints > 0)
                    {
                        int updatedPoints = currentRewardPoints - redeemPoints + earnedPoints;
                        string updatedRank = currentMembershipRank;
                        if (updatedPoints >= 1000) updatedRank = "Kim Cương";
                        else if (updatedPoints >= 500) updatedRank = "Vàng";
                        else if (updatedPoints >= 100) updatedRank = "Bạc";

                        using var updateCustCmd = connection.CreateCommand();
                        updateCustCmd.Transaction = transaction;
                        updateCustCmd.CommandText = @"
                            UPDATE Customers
                            SET RewardPoints = @RewardPoints, MembershipRank = @MembershipRank
                            WHERE CustomerId = @CustomerId";

                        updateCustCmd.Parameters.Add(new SqlParameter("@RewardPoints", SqlDbType.Int) { Value = updatedPoints });
                        updateCustCmd.Parameters.Add(new SqlParameter("@MembershipRank", SqlDbType.NVarChar, 50) { Value = updatedRank });
                        updateCustCmd.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId.Value });

                        await updateCustCmd.ExecuteNonQueryAsync();
                    }
                }

                // 7. Xác nhận giao dịch thành công (Commit Transaction)
                await transaction.CommitAsync();

                return Ok(new
                {
                    orderId = newOrderId,
                    totalAmount = finalAmount,
                    originalAmount = totalAmount,
                    discountAmount = discountAmount,
                    redeemedPoints = redeemPoints,
                    customerName = customerName ?? "Khách vãng lai",
                    message = "Thanh toán thành công."
                });
            }
            catch (Exception ex)
            {
                // Khi xảy ra bất kỳ lỗi nào hoặc hết hàng -> Rollback toàn bộ giao dịch để bảo toàn ACID (TC35)
                try
                {
                    await transaction.RollbackAsync();
                }
                catch
                {
                    // Nuốt lỗi rollback nếu transaction đã bị hủy trước đó
                }

                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // Báo cáo / thống kê đơn hàng theo khoảng ngày
        [HttpGet("summary")]
        [HttpGet("report")]
        public async Task<IActionResult> GetReport(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            DateTime start = (fromDate?.Date ?? DateTime.Today.AddDays(-30));
            DateTime end = (toDate?.Date ?? DateTime.Today).AddDays(1).AddTicks(-1);

            var ordersQuery = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.OrderDate >= start && o.OrderDate <= end);

            var orders = await ordersQuery
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            decimal totalRevenue = orders
                .Where(o => !string.Equals(o.Status, "Cancelled", StringComparison.OrdinalIgnoreCase))
                .Sum(o => o.TotalAmount);

            int totalOrders = orders.Count;

            int totalProducts = orders
                .SelectMany(o => o.OrderItems ?? Enumerable.Empty<OrderDetail>())
                .Sum(oi => oi.Quantity);

            var recentOrders = orders.Select(o => new ReportOrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                CustomerName = o.Customer != null ? o.Customer.FullName : "Khách vãng lai",
                CashierName = o.User != null ? (!string.IsNullOrEmpty(o.User.FullName) ? o.User.FullName : o.User.Username) : "Thu ngân",
                TotalItems = o.OrderItems != null ? o.OrderItems.Sum(oi => oi.Quantity) : 0,
                TotalAmount = o.TotalAmount,
                Status = string.IsNullOrWhiteSpace(o.Status) ? "Completed" : o.Status
            }).ToList();

            var summary = new ReportSummaryDto
            {
                FromDate = start,
                ToDate = end,
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalProducts = totalProducts,
                RecentOrders = recentOrders
            };

            return Ok(summary);
        }

        private sealed class CheckoutItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }

    public sealed class CheckoutRequest
    {
        public string? CashierUsername { get; set; }
        public string? CustomerPhone { get; set; }
        public string? PaymentMethod { get; set; }
        public int RedeemPoints { get; set; }
        public List<CheckoutRequestItem> Items { get; set; } = new List<CheckoutRequestItem>();
        public List<CheckoutRequestItem>? Details { get; set; }
    }

    public sealed class CheckoutRequestItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
