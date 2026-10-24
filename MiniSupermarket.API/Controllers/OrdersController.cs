using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public OrdersController(SupermarketDbContext context)
        {
            _context = context;
        }

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

            string username = (request.CashierUsername ?? string.Empty).Trim();
            User? cashier = null;
            if (!string.IsNullOrWhiteSpace(username))
            {
                cashier = await _context.Users
                    .FirstOrDefaultAsync(user => user.Username == username);
            }

            if (cashier == null)
            {
                cashier = await _context.Users.FirstOrDefaultAsync();
            }

            if (cashier == null)
                return Unauthorized(new { message = "Không xác định được tài khoản thu ngân." });

            Customer? customer = null;
            string phone = (request.CustomerPhone ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                customer = await _context.Customers
                    .FirstOrDefaultAsync(item => item.PhoneNumber == phone || (item.FullName != null && item.FullName.ToLower() == phone.ToLower()));
            }

            int[] productIds = items
                .Select(item => item.ProductId)
                .Distinct()
                .ToArray();

            List<Product> products = await _context.Products
                .Where(product => productIds.Contains(product.ProductId))
                .ToListAsync();

            if (products.Count != productIds.Length)
                return BadRequest(new { message = "Một hoặc nhiều sản phẩm không còn tồn tại." });

            Dictionary<int, CheckoutItem> requestedItems = items
                .GroupBy(item => item.ProductId)
                .ToDictionary(
                    group => group.Key,
                    group => new CheckoutItem
                    {
                        ProductId = group.Key,
                        Quantity = group.Sum(item => item.Quantity)
                    });

            foreach (Product product in products)
            {
                int quantity = requestedItems[product.ProductId].Quantity;
                if (product.StockQuantity < quantity)
                {
                    return BadRequest(new
                    {
                        message = $"Sản phẩm '{product.ProductName}' không đủ tồn kho. Còn lại: {product.StockQuantity}."
                    });
                }
            }

            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                Order order = new Order
                {
                    OrderDate = DateTime.Now,
                    Status = "Completed",
                    CustomerId = customer?.CustomerId,
                    UserId = cashier.UserId,
                    OrderItems = new List<OrderItem>()
                };

                foreach (Product product in products)
                {
                    int quantity = requestedItems[product.ProductId].Quantity;
                    product.StockQuantity -= quantity;
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    });
                    order.TotalAmount += product.Price * quantity;
                }

                // Tích lũy điểm cho khách hàng (1 điểm mỗi 10.000đ)
                if (customer != null)
                {
                    int earnedPoints = (int)(order.TotalAmount / 10000);
                    if (earnedPoints > 0)
                    {
                        customer.RewardPoints = (customer.RewardPoints ?? 0) + earnedPoints;
                        if (customer.RewardPoints >= 1000) customer.MembershipRank = "Kim Cương";
                        else if (customer.RewardPoints >= 500) customer.MembershipRank = "Vàng";
                        else if (customer.RewardPoints >= 100) customer.MembershipRank = "Bạc";
                    }
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    orderId = order.OrderId,
                    totalAmount = order.TotalAmount,
                    customerName = customer?.FullName ?? "Khách vãng lai",
                    message = "Thanh toán thành công."
                });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
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
                .SelectMany(o => o.OrderItems ?? Enumerable.Empty<OrderItem>())
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