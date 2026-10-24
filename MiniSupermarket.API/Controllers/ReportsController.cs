using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public ReportsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // GET: api/reports?fromDate=2026-10-01&toDate=2026-10-23
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

        // GET: api/reports/summary?fromDate=...&toDate=...
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            return await GetReport(fromDate, toDate);
        }
    }
}
