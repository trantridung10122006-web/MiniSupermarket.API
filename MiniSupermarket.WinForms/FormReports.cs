using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    internal class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }
    }

    public partial class FormReports : Form
    {
        private ReportSummaryDto _currentReport;
        private Label _lblNewCustomers;
        private Label _lblLowStock;
        private ListBox _lstBestSellers;
        private readonly List<RevenueTrendDto> _revenueTrend = new List<RevenueTrendDto>();
        private readonly List<CategoryRevenueDto> _categoryRevenue = new List<CategoryRevenueDto>();
        private Panel _trendPanel;
        private Panel _categoryPanel;

        public FormReports()
        {
            InitializeComponent();
            SetupGrid();
            WireEvents();
            WinFormsTheme.Apply(this);
            ApplyCustomStyles();
            BuildRevenueDashboard();
        }

        // =====================================================
        // 1. CẤU HÌNH BẢNG & SỰ KIỆN
        // =====================================================
        private void ApplyCustomStyles()
        {
            _lblNewCustomers = lblProducts;
            _lblLowStock = lblLowStock;
            _lstBestSellers = lstBestSellers;

            // Đặt màu phân biệt rõ ràng cho các nút tác vụ (không bị theme ghi đè)
            btnViewReport.BackColor = Color.FromArgb(0, 121, 107);
            btnRefresh.BackColor = Color.FromArgb(96, 125, 139);
            btnExport.BackColor = Color.FromArgb(46, 125, 50);

            // Màu tiêu đề bảng đồng bộ nhận diện thương hiệu
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Đăng ký vẽ đẹp mắt cho Top bán chạy
            lstBestSellers.DrawItem -= LstBestSellers_DrawItem;
            lstBestSellers.DrawItem += LstBestSellers_DrawItem;
        }

        private void BuildRevenueDashboard()
        {
            _trendPanel = pnlTrendChart;
            _categoryPanel = pnlCategoryChart;

            _trendPanel.Paint += PaintTrendChart;
            _categoryPanel.Paint += PaintCategoryChart;
        }

        private void SetupGrid()
        {
            dgvReports.AutoGenerateColumns = false;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.ReadOnly = true;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.MultiSelect = false;
            dgvReports.BackgroundColor = Color.White;
            dgvReports.BorderStyle = BorderStyle.None;
            dgvReports.RowHeadersVisible = false;
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvReports.ColumnHeadersHeight = 34;
            dgvReports.RowTemplate.Height = 30;
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvReports.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvReports.GridColor = Color.FromArgb(226, 232, 240);

            dgvReports.Columns.Clear();

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.OrderId),
                HeaderText = "Mã đơn",
                Width = 85,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.OrderDate),
                HeaderText = "Ngày tạo",
                Width = 145,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "dd/MM/yyyy HH:mm"
                }
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.CustomerName),
                HeaderText = "Khách hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 180
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.CashierName),
                HeaderText = "Thu ngân",
                Width = 160
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.TotalItems),
                HeaderText = "Số lượng SP",
                Width = 115,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.TotalAmount),
                HeaderText = "Tổng tiền",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0",
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                }
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.Status),
                HeaderText = "Trạng thái",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
        }

        private void WireEvents()
        {
            this.Load += FormReports_Load;
            btnViewReport.Click += async (s, e) => { await LoadReportsAsync(); await LoadDashboardExtrasAsync(); };
            btnRefresh.Click += async (s, e) => { await ResetAndLoadAsync(); await LoadDashboardExtrasAsync(); };
            btnExport.Click += (s, e) => ExportReport();
        }

        private async void FormReports_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;
            await LoadReportsAsync();
            await LoadDashboardExtrasAsync();
        }

        private async Task LoadDashboardExtrasAsync()
        {
            try
            {
                var dashboard = await ApiClientService.Client.GetFromJsonAsync<DashboardSummaryDto>("reports/dashboard-summary");
                if (dashboard != null)
                {
                    // Cập nhật số khách mới và mặt hàng sắp hết từ dashboard
                    _lblNewCustomers.Text = dashboard.NewCustomers.ToString("N0");
                    _lblLowStock.Text = dashboard.LowStockProducts.ToString("N0");
                }

                string from = dtpFromDate.Value.ToString("yyyy-MM-dd");
                string to = dtpToDate.Value.ToString("yyyy-MM-dd");
                var bestSellers = await ApiClientService.Client.GetFromJsonAsync<List<BestSellerDto>>($"reports/best-sellers?fromDate={from}&toDate={to}");
                _lstBestSellers.Items.Clear();
                if (bestSellers != null && bestSellers.Count > 0)
                {
                    int rank = 1;
                    foreach (var item in bestSellers)
                    {
                        _lstBestSellers.Items.Add($"  {rank++}. {item.ProductName}  —  Đã bán: {item.Quantity:N0} sản phẩm  |  Doanh thu: {item.Revenue:N0} đ");
                    }
                }
                else
                {
                    _lstBestSellers.Items.Add("  (Chưa có dữ liệu sản phẩm bán chạy trong khoảng thời gian đã chọn)");
                }

                _revenueTrend.Clear();
                var trend = await ApiClientService.Client.GetFromJsonAsync<List<RevenueTrendDto>>(
                    $"reports/revenue-trend?fromDate={from}&toDate={to}");
                if (trend != null) _revenueTrend.AddRange(trend);

                _categoryRevenue.Clear();
                var categories = await ApiClientService.Client.GetFromJsonAsync<List<CategoryRevenueDto>>(
                    $"reports/category-revenue?fromDate={from}&toDate={to}");
                if (categories != null) _categoryRevenue.AddRange(categories);

                _trendPanel?.Invalidate();
                _categoryPanel?.Invalidate();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Không tải được KPI/Top sản phẩm: " + ex.Message;
            }
        }

        private async Task ResetAndLoadAsync()
        {
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;
            await LoadReportsAsync();
        }

        private void PaintTrendChart(object sender, PaintEventArgs e)
        {
            ChartPainter.DrawRevenueTrend(e.Graphics, _trendPanel.ClientRectangle, _revenueTrend);
        }

        private void PaintCategoryChart(object sender, PaintEventArgs e)
        {
            ChartPainter.DrawCategoryShare(e.Graphics, _categoryPanel.ClientRectangle, _categoryRevenue);
        }

        private void LstBestSellers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstBestSellers.Items.Count) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var bounds = e.Bounds;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color bg = isSelected ? Color.FromArgb(224, 242, 254) : (e.Index % 2 == 0 ? Color.White : Color.FromArgb(248, 250, 252));
            using (var b = new SolidBrush(bg))
                g.FillRectangle(b, bounds);

            string text = lstBestSellers.Items[e.Index]?.ToString() ?? "";
            if (text.StartsWith("  (") || !text.Contains("—"))
            {
                using (var f = new Font("Segoe UI", 8.5F, FontStyle.Italic))
                using (var b = new SolidBrush(Color.DimGray))
                    g.DrawString(text, f, b, bounds.Left + 8, bounds.Top + 9);
                return;
            }

            int rank = e.Index + 1;
            Color badgeColor;
            if (rank == 1) badgeColor = Color.FromArgb(245, 158, 11);     // Gold
            else if (rank == 2) badgeColor = Color.FromArgb(148, 163, 184); // Silver
            else if (rank == 3) badgeColor = Color.FromArgb(217, 119, 6);   // Bronze
            else badgeColor = Color.FromArgb(203, 213, 225);                // Slate

            // Vẽ badge tròn số thứ tự
            var badgeRect = new Rectangle(bounds.Left + 8, bounds.Top + (bounds.Height - 20) / 2, 20, 20);
            using (var b = new SolidBrush(badgeColor))
                g.FillEllipse(b, badgeRect);

            using (var f = new Font("Segoe UI", 8F, FontStyle.Bold))
            {
                string rankStr = rank.ToString();
                var size = g.MeasureString(rankStr, f);
                g.DrawString(rankStr, f, rank <= 3 ? Brushes.White : Brushes.Black,
                    badgeRect.X + (badgeRect.Width - size.Width) / 2 + 0.5f,
                    badgeRect.Y + (badgeRect.Height - size.Height) / 2);
            }

            // Tách tên và doanh thu
            string[] parts = text.Split(new[] { "—" }, StringSplitOptions.None);
            string title = parts[0].Trim();
            if (title.StartsWith($"{rank}.")) title = title.Substring($"{rank}.".Length).Trim();
            else if (title.StartsWith($"  {rank}.")) title = title.Substring($"  {rank}.".Length).Trim();

            string sub = parts.Length > 1 ? parts[1].Trim() : "";

            using (var fontTitle = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (var fontSub = new Font("Segoe UI", 8F))
            using (var brushTitle = new SolidBrush(Color.FromArgb(30, 41, 59)))
            using (var brushSub = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                int textLeft = badgeRect.Right + 8;
                int textW = bounds.Right - textLeft - 8;
                g.DrawString(title, fontTitle, brushTitle, new RectangleF(textLeft, bounds.Top + 2, textW, 16));
                g.DrawString(sub, fontSub, brushSub, new RectangleF(textLeft, bounds.Top + 18, textW, 14));
            }

            // Đường phân cách mờ ở đáy
            using (var pen = new Pen(Color.FromArgb(241, 245, 249)))
                g.DrawLine(pen, bounds.Left + 8, bounds.Bottom - 1, bounds.Right - 8, bounds.Bottom - 1);
        }

        // =====================================================
        // 2. KẾT NỐI API & TẢI DỮ LIỆU BÁO CÁO
        // =====================================================
        private async Task LoadReportsAsync()
        {
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                MessageBox.Show("Từ ngày không được lớn hơn đến ngày!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFromDate.Focus();
                return;
            }

            SetBusy(true);
            lblStatus.Text = "Đang tải dữ liệu báo cáo từ máy chủ...";

            string fromStr = dtpFromDate.Value.ToString("yyyy-MM-dd");
            string toStr = dtpToDate.Value.ToString("yyyy-MM-dd");

            try
            {
                ReportSummaryDto summary = null;

                try
                {
                    summary = await ApiClientService.Client
                        .GetFromJsonAsync<ReportSummaryDto>($"reports?fromDate={fromStr}&toDate={toStr}");
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("404"))
                {
                    // Fallback sang endpoint orders/summary nếu reports chưa sẵn sàng
                    summary = await ApiClientService.Client
                        .GetFromJsonAsync<ReportSummaryDto>($"orders/summary?fromDate={fromStr}&toDate={toStr}");
                }

                if (summary != null)
                {
                    _currentReport = summary;

                    lblRevenue.Text = $"{summary.TotalRevenue:N0} đ";
                    lblOrders.Text = summary.TotalOrders.ToString("N0");

                    dgvReports.DataSource = summary.RecentOrders ?? new List<ReportOrderDto>();

                    lblGridSummary.Text = $"Tìm thấy {summary.TotalOrders:N0} hóa đơn trong kỳ lọc";
                    lblStatus.Text = $"Cập nhật lúc: {DateTime.Now:HH:mm:ss} | Tìm thấy {summary.TotalOrders:N0} hóa đơn | Doanh thu: {summary.TotalRevenue:N0} đ";
                    lblStatus.ForeColor = Color.DimGray;
                }
                else
                {
                    lblStatus.Text = "Không có dữ liệu báo cáo trong khoảng thời gian này.";
                    lblGridSummary.Text = "0 hóa đơn";
                }
            }
            catch (HttpRequestException ex)
            {
                lblStatus.Text = "Không thể kết nối máy chủ để lấy báo cáo.";
                lblStatus.ForeColor = Color.Firebrick;
                MessageBox.Show(
                    "Không thể kết nối API để tải báo cáo.\n\n" + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                lblStatus.Text = "Yêu cầu hết thời gian chờ.";
                lblStatus.ForeColor = Color.Firebrick;
                MessageBox.Show(
                    "Yêu cầu tải báo cáo đã hết thời gian chờ. Vui lòng thử lại!",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khi tải báo cáo: " + ex.Message;
                lblStatus.ForeColor = Color.Firebrick;
                MessageBox.Show(
                    "Đã xảy ra lỗi khi tải dữ liệu báo cáo.\n\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            UseWaitCursor = busy;
            btnViewReport.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            btnExport.Enabled = !busy;
            dtpFromDate.Enabled = !busy;
            dtpToDate.Enabled = !busy;
        }

        // =====================================================
        // 3. XUẤT BÁO CÁO (EXCEL / CSV)
        // =====================================================
        private void ExportReport()
        {
            if (_currentReport == null || _currentReport.RecentOrders == null || _currentReport.RecentOrders.Count == 0)
            {
                MessageBox.Show(
                    "Không có dữ liệu đơn hàng trong khoảng ngày đã chọn để xuất báo cáo!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuất báo cáo bán hàng";
                sfd.Filter = "File Excel CSV (*.csv)|*.csv|Bảng tính Excel (*.xls)|*.xls";
                sfd.FileName = $"BaoCao_DoanhThu_{dtpFromDate.Value:yyyyMMdd}_{dtpToDate.Value:yyyyMMdd}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string ext = Path.GetExtension(sfd.FileName).ToLowerInvariant();
                        if (ext == ".xls")
                        {
                            ExportToHtmlExcel(sfd.FileName);
                        }
                        else
                        {
                            ExportToCsv(sfd.FileName);
                        }

                        var result = MessageBox.Show(
                            "Xuất file báo cáo thành công!\n\nĐường dẫn: " + sfd.FileName + "\n\nBạn có muốn mở file ngay không?",
                            "Thành công",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(
                                    "Không thể mở file tự động: " + ex.Message,
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Lỗi khi xuất file báo cáo: " + ex.Message,
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportToCsv(string filePath)
        {
            // Ghi file CSV với UTF-8 BOM để Excel hiển thị đúng Tiếng Việt có dấu
            using (var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)))
            {
                writer.WriteLine("BÁO CÁO THỐNG KÊ DOANH THU BÁN HÀNG");
                writer.WriteLine($"Thời gian báo cáo:,\"Từ {dtpFromDate.Value:dd/MM/yyyy} đến {dtpToDate.Value:dd/MM/yyyy}\"");
                writer.WriteLine($"Ngày xuất báo cáo:,\"{DateTime.Now:dd/MM/yyyy HH:mm:ss}\"");
                writer.WriteLine($"Tổng doanh thu:,\"{_currentReport.TotalRevenue:N0} đ\"");
                writer.WriteLine($"Tổng số lượng hóa đơn:,\"{_currentReport.TotalOrders:N0}\"");
                writer.WriteLine($"Tổng số sản phẩm đã bán:,\"{_currentReport.TotalProducts:N0}\"");
                writer.WriteLine();

                // Tiêu đề cột
                writer.WriteLine("Mã đơn,Ngày tạo,Khách hàng,Thu ngân,Số lượng sản phẩm,Tổng tiền (VNĐ),Trạng thái");

                // Dữ liệu từng dòng
                foreach (var item in _currentReport.RecentOrders)
                {
                    string customer = EscapeCsv(item.CustomerName);
                    string cashier = EscapeCsv(item.CashierName);
                    string status = EscapeCsv(item.Status);

                    writer.WriteLine($"{item.OrderId},\"{item.OrderDate:dd/MM/yyyy HH:mm}\",\"{customer}\",\"{cashier}\",{item.TotalItems},{item.TotalAmount},\"{status}\"");
                }
            }
        }

        private void ExportToHtmlExcel(string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\">");
            sb.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h2 style=\"color:#234B82;text-align:center;\">BÁO CÁO THỐNG KÊ DOANH THU BÁN HÀNG</h2>");
            sb.AppendLine($"<p style=\"text-align:center;\"><b>Khoảng thời gian:</b> {dtpFromDate.Value:dd/MM/yyyy} - {dtpToDate.Value:dd/MM/yyyy}</p>");
            sb.AppendLine($"<p style=\"text-align:center;\"><b>Thời điểm xuất:</b> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");

            sb.AppendLine("<table border=\"1\" cellpadding=\"5\" cellspacing=\"0\" style=\"border-collapse:collapse;margin-bottom:15px;\">");
            sb.AppendLine($"<tr><td style=\"background:#eef;\"><b>Tổng doanh thu:</b></td><td><b>{_currentReport.TotalRevenue:N0} đ</b></td></tr>");
            sb.AppendLine($"<tr><td style=\"background:#eef;\"><b>Số hóa đơn:</b></td><td>{_currentReport.TotalOrders:N0}</td></tr>");
            sb.AppendLine($"<tr><td style=\"background:#eef;\"><b>Số sản phẩm đã bán:</b></td><td>{_currentReport.TotalProducts:N0}</td></tr>");
            sb.AppendLine("</table>");

            sb.AppendLine("<table border=\"1\" cellpadding=\"5\" cellspacing=\"0\" style=\"border-collapse:collapse;width:100%;\">");
            sb.AppendLine("<tr style=\"background-color:#234B82;color:white;font-weight:bold;text-align:center;\">");
            sb.AppendLine("<th>Mã đơn</th><th>Ngày tạo</th><th>Khách hàng</th><th>Thu ngân</th><th>Số lượng SP</th><th>Tổng tiền (VNĐ)</th><th>Trạng thái</th>");
            sb.AppendLine("</tr>");

            foreach (var item in _currentReport.RecentOrders)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td style=\"text-align:center;\">{item.OrderId}</td>");
                sb.AppendLine($"<td style=\"text-align:center;\">{item.OrderDate:dd/MM/yyyy HH:mm}</td>");
                sb.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(item.CustomerName)}</td>");
                sb.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(item.CashierName)}</td>");
                sb.AppendLine($"<td style=\"text-align:right;\">{item.TotalItems:N0}</td>");
                sb.AppendLine($"<td style=\"text-align:right;font-weight:bold;\">{item.TotalAmount:N0}</td>");
                sb.AppendLine($"<td style=\"text-align:center;\">{System.Net.WebUtility.HtmlEncode(item.Status)}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Replace("\"", "\"\"");
        }
    }

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

    public class BestSellerDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
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

    internal static class ChartPainter
    {
        private static readonly Color[] Colors =
        {
            Color.FromArgb(13, 148, 136),  // Teal
            Color.FromArgb(37, 99, 235),   // Blue
            Color.FromArgb(245, 158, 11),  // Amber
            Color.FromArgb(139, 92, 246),  // Purple
            Color.FromArgb(239, 68, 68),   // Red
            Color.FromArgb(6, 182, 212)    // Cyan
        };

        public static void DrawRevenueTrend(Graphics g, Rectangle bounds, List<RevenueTrendDto> data)
        {
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (var titleFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var subFont = new Font("Segoe UI", 8F))
            using (var axisPen = new Pen(Color.FromArgb(203, 213, 225), 1))
            using (var gridPen = new Pen(Color.FromArgb(241, 245, 249), 1) { DashStyle = DashStyle.Dash })
            using (var barBrush = new SolidBrush(Color.FromArgb(0, 137, 123)))
            using (var titleBrush = new SolidBrush(Color.FromArgb(30, 41, 59)))
            using (var labelBrush = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                g.DrawString("📊  DOANH THU THEO NGÀY", titleFont, titleBrush, 10, 8);

                if (data != null && data.Count > 0)
                {
                    decimal totalRevenue = data.Sum(x => x.Revenue);
                    string totalStr = $"Tổng kỳ: {totalRevenue:N0} đ";
                    var size = g.MeasureString(totalStr, subFont);
                    g.DrawString(totalStr, subFont, labelBrush, bounds.Width - size.Width - 12, 9);
                }

                int left = 48;
                int top = 34;
                int right = bounds.Width - 14;
                int bottom = bounds.Height - 26;
                int plotW = right - left;
                int plotH = bottom - top;

                if (plotW <= 20 || plotH <= 20) return;

                // Đường trục hoành đáy
                g.DrawLine(axisPen, left, bottom, right, bottom);

                if (data == null || data.Count == 0)
                {
                    string emptyMsg = "Chưa có dữ liệu doanh thu trong khoảng thời gian đã chọn";
                    var sz = g.MeasureString(emptyMsg, subFont);
                    g.DrawString(emptyMsg, subFont, labelBrush, left + (plotW - sz.Width) / 2, top + (plotH - sz.Height) / 2);
                    return;
                }

                decimal max = data.Max(x => x.Revenue);
                if (max <= 0) max = 1;

                // 2 đường lưới tham chiếu (Max & 50%)
                g.DrawLine(gridPen, left, top + 6, right, top + 6);
                g.DrawString(FormatCompact(max), subFont, labelBrush, 2, top + 2);

                g.DrawLine(gridPen, left, top + plotH / 2, right, top + plotH / 2);
                g.DrawString(FormatCompact(max / 2), subFont, labelBrush, 2, top + plotH / 2 - 6);

                float slot = (float)plotW / data.Count;
                float barW = Math.Max(4, Math.Min(32, slot * 0.65f));
                float offset = (slot - barW) / 2f;

                for (int i = 0; i < data.Count; i++)
                {
                    float barH = (float)(data[i].Revenue / max) * (plotH - 14);
                    if (barH < 2 && data[i].Revenue > 0) barH = 2;

                    float barX = left + i * slot + offset;
                    float barY = bottom - barH;

                    var barRect = new RectangleF(barX, barY, barW, barH);
                    g.FillRectangle(barBrush, barRect);

                    // Hiển thị giá trị trên đỉnh cột nếu ít ngày
                    if (data.Count <= 14 && data[i].Revenue > 0)
                    {
                        string valStr = FormatCompact(data[i].Revenue);
                        var valSz = g.MeasureString(valStr, subFont);
                        g.DrawString(valStr, subFont, labelBrush, barX + (barW - valSz.Width) / 2, barY - 14);
                    }

                    // Nhãn ngày
                    if (data.Count <= 14 || i % Math.Max(1, data.Count / 8) == 0)
                    {
                        string dtStr = data[i].Date.ToString("dd/MM");
                        var dtSz = g.MeasureString(dtStr, subFont);
                        g.DrawString(dtStr, subFont, labelBrush, barX + (barW - dtSz.Width) / 2, bottom + 4);
                    }
                }
            }
        }

        public static void DrawCategoryShare(Graphics g, Rectangle bounds, List<CategoryRevenueDto> data)
        {
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (var titleFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var itemFont = new Font("Segoe UI", 8F))
            using (var pctFont = new Font("Segoe UI", 8F, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.FromArgb(30, 41, 59)))
            using (var labelBrush = new SolidBrush(Color.FromArgb(71, 85, 105)))
            {
                g.DrawString("🍩  TỈ TRỌNG THEO NGÀNH HÀNG", titleFont, titleBrush, 10, 8);

                decimal total = data == null ? 0 : data.Sum(x => x.Revenue);
                if (total <= 0 || data == null || data.Count == 0)
                {
                    string emptyMsg = "Chưa có dữ liệu ngành hàng trong khoảng thời gian này";
                    var sz = g.MeasureString(emptyMsg, itemFont);
                    g.DrawString(emptyMsg, itemFont, labelBrush, (bounds.Width - sz.Width) / 2, (bounds.Height - sz.Height) / 2);
                    return;
                }

                // Kích thước donut chart thích ứng
                int diameter = Math.Min(bounds.Height - 50, Math.Min(135, bounds.Width / 2 - 20));
                if (diameter < 60) diameter = 60;

                int pieX = 14;
                int pieY = 32 + (bounds.Height - 32 - diameter) / 2;
                var pieRect = new Rectangle(pieX, pieY, diameter, diameter);

                float startAngle = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    float sweep = (float)(data[i].Revenue / total * 360m);
                    if (sweep < 1f && data[i].Revenue > 0) sweep = 1f;

                    using (var brush = new SolidBrush(Colors[i % Colors.Length]))
                    {
                        g.FillPie(brush, pieRect, startAngle, sweep);
                    }
                    startAngle += sweep;
                }

                // Lỗ tròn ở giữa donut
                int holeSize = (int)(diameter * 0.52f);
                int holeX = pieRect.X + (diameter - holeSize) / 2;
                int holeY = pieRect.Y + (diameter - holeSize) / 2;
                using (var holeBrush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(holeBrush, holeX, holeY, holeSize, holeSize);
                }

                // Danh mục chú thích bên phải
                int legendLeft = pieRect.Right + 12;
                int legendTop = Math.Max(32, pieY);
                int maxItems = Math.Min(data.Count, 6);

                for (int i = 0; i < maxItems; i++)
                {
                    int rowY = legendTop + i * 22;
                    if (rowY + 18 > bounds.Height) break;

                    using (var brush = new SolidBrush(Colors[i % Colors.Length]))
                    {
                        g.FillRectangle(brush, legendLeft, rowY + 3, 10, 10);
                    }

                    string name = data[i].CategoryName;
                    if (name.Length > 15) name = name.Substring(0, 15) + "…";

                    float pct = (float)(data[i].Revenue / total);
                    g.DrawString(name, itemFont, labelBrush, legendLeft + 14, rowY);

                    string pctStr = $"{pct:P0}";
                    g.DrawString(pctStr, pctFont, Brushes.DimGray, bounds.Width - 40, rowY);
                }
            }
        }

        private static string FormatCompact(decimal val)
        {
            if (val >= 1_000_000_000) return $"{(val / 1_000_000_000m):0.#}B";
            if (val >= 1_000_000) return $"{(val / 1_000_000m):0.#}M";
            if (val >= 1_000) return $"{(val / 1_000m):0.#}K";
            return val.ToString("N0");
        }
    }
}
