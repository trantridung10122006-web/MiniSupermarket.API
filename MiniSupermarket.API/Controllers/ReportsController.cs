using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;
using System.Data;

namespace MiniSupermarket.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public ReportsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/reports?fromDate=2026-10-01&toDate=2026-10-23
        [HttpGet]
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

        // 2. GET: api/reports/summary?fromDate=...&toDate=...
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            return await GetReport(fromDate, toDate);
        }

        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var todayOrders = _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= today && o.OrderDate < tomorrow && o.Status != "Cancelled");

            var result = new DashboardSummaryDto
            {
                TodayRevenue = await todayOrders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0m,
                TotalOrders = await todayOrders.CountAsync(),
                NewCustomers = await _context.Customers.AsNoTracking()
                    .CountAsync(c => c.CreatedAt >= today && c.CreatedAt < tomorrow),
                LowStockProducts = await _context.Products.AsNoTracking()
                    .CountAsync(p => p.StockQuantity <= 10)
            };

            return Ok(result);
        }

        [HttpGet("revenue-trend")]
        public async Task<IActionResult> GetRevenueTrend(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            DateTime start = (fromDate?.Date ?? DateTime.Today.AddDays(-6));
            DateTime end = (toDate?.Date ?? DateTime.Today).AddDays(1);

            var orders = await _context.Orders.AsNoTracking()
                .Where(o => o.OrderDate >= start && o.OrderDate < end && o.Status != "Cancelled")
                .Select(o => new { o.OrderDate, o.TotalAmount })
                .ToListAsync();

            var trend = orders.GroupBy(o => o.OrderDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new RevenueTrendDto
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount)
                });

            return Ok(trend);
        }

        [HttpGet("category-revenue")]
        public async Task<IActionResult> GetCategoryRevenue(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            DateTime start = (fromDate?.Date ?? DateTime.Today.AddDays(-30));
            DateTime end = (toDate?.Date ?? DateTime.Today).AddDays(1);

            var result = await _context.OrderItems
                .AsNoTracking()
                .Where(d => d.Order != null && d.Order.OrderDate >= start && d.Order.OrderDate < end && d.Order.Status != "Cancelled")
                .GroupBy(d => new
                {
                    CategoryId = d.Product != null ? d.Product.CategoryId : 0,
                    CategoryName = d.Product != null && d.Product.Category != null ? d.Product.Category.CategoryName : "Chưa phân loại"
                })
                .Select(g => new CategoryRevenueDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    Revenue = g.Sum(d => d.Quantity * d.UnitPrice)
                })
                .OrderByDescending(x => x.Revenue)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellers(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            DateTime start = (fromDate?.Date ?? DateTime.Today.AddDays(-30));
            DateTime end = (toDate?.Date ?? DateTime.Today).AddDays(1);

            var bestSellers = await _context.OrderItems
                .AsNoTracking()
                .Where(d => d.Order != null && d.Order.OrderDate >= start && d.Order.OrderDate < end && d.Order.Status != "Cancelled")
                .GroupBy(d => new { d.ProductId, ProductName = d.Product != null ? d.Product.ProductName : "Không xác định" })
                .Select(g => new BestSellerDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(d => d.Quantity),
                    Revenue = g.Sum(d => d.Quantity * d.UnitPrice)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

            return Ok(bestSellers);
        }

        // 3. GET: api/reports/shift-revenue (Thực thi Stored Procedure sp_GetRevenueByCashier bằng CommandType.StoredProcedure - TC34)
        [HttpGet("shift-revenue")]
        [HttpGet("cashier-revenue")]
        public async Task<IActionResult> GetShiftRevenue(
            [FromQuery] string? cashierUsername,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var connectionString = _context.Database.GetConnectionString();
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_GetRevenueByCashier";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@CashierUsername", SqlDbType.NVarChar, 100)
            {
                Value = string.IsNullOrWhiteSpace(cashierUsername) ? DBNull.Value : cashierUsername.Trim()
            });

            command.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime2)
            {
                Value = fromDate.HasValue ? fromDate.Value : DBNull.Value
            });

            command.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime2)
            {
                Value = toDate.HasValue ? toDate.Value : DBNull.Value
            });

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new
                {
                    cashierUsername = reader["CashierUsername"]?.ToString(),
                    cashierFullName = reader["CashierFullName"] == DBNull.Value ? null : reader["CashierFullName"]?.ToString(),
                    totalOrders = Convert.ToInt32(reader["TotalOrders"]),
                    totalRevenue = Convert.ToDecimal(reader["TotalRevenue"]),
                    fromDate = reader["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FromDate"]),
                    toDate = reader["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ToDate"])
                };

                return Ok(result);
            }

            return Ok(new
            {
                cashierUsername = cashierUsername ?? "Tất cả",
                cashierFullName = (string?)null,
                totalOrders = 0,
                totalRevenue = 0m,
                fromDate = fromDate,
                toDate = toDate
            });
        }
    }
}
