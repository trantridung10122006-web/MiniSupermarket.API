using System;
using System.Collections.Generic;

namespace MiniSupermarket.API.Models
{
    public class ReportSummaryDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public List<ReportOrderDto> RecentOrders { get; set; } = new List<ReportOrderDto>();
    }

    public class ReportOrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CashierName { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DashboardSummaryDto
    {
        public decimal TodayRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewCustomers { get; set; }
        public int LowStockProducts { get; set; }
    }

    public class RevenueTrendDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
    }

    public class CategoryRevenueDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class BestSellerDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
