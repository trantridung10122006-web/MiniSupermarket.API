using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormReports : Form
    {
        private ReportSummaryDto _currentReport;

        public FormReports()
        {
            InitializeComponent();
            SetupGrid();
            WireEvents();
            WinFormsTheme.Apply(this);
        }

        // =====================================================
        // 1. CẤU HÌNH BẢNG & SỰ KIỆN
        // =====================================================
        private void SetupGrid()
        {
            dgvReports.AutoGenerateColumns = false;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.ReadOnly = true;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.MultiSelect = false;
            dgvReports.BackgroundColor = Color.White;
            dgvReports.BorderStyle = BorderStyle.FixedSingle;
            dgvReports.RowHeadersVisible = false;
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 75, 130);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvReports.ColumnHeadersHeight = 38;
            dgvReports.RowTemplate.Height = 32;
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252);
            dgvReports.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgvReports.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvReports.GridColor = Color.FromArgb(224, 224, 224);

            dgvReports.Columns.Clear();

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.OrderId),
                HeaderText = "Mã đơn",
                Width = 85
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.OrderDate),
                HeaderText = "Ngày tạo",
                Width = 145,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy HH:mm"
                }
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.CustomerName),
                HeaderText = "Khách hàng",
                Width = 180
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.CashierName),
                HeaderText = "Thu ngân",
                Width = 150
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.TotalItems),
                HeaderText = "Số lượng SP",
                Width = 110,
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
                Width = 135,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                }
            });

            dgvReports.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ReportOrderDto.Status),
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void WireEvents()
        {
            this.Load += FormReports_Load;
            btnViewReport.Click += async (s, e) => await LoadReportsAsync();
            btnRefresh.Click += async (s, e) => await ResetAndLoadAsync();
            btnExport.Click += (s, e) => ExportReport();
        }

        private async void FormReports_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;
            await LoadReportsAsync();
        }

        private async Task ResetAndLoadAsync()
        {
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;
            await LoadReportsAsync();
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
                    lblProducts.Text = summary.TotalProducts.ToString("N0");

                    dgvReports.DataSource = summary.RecentOrders ?? new List<ReportOrderDto>();

                    lblStatus.Text = $"Cập nhật lúc: {DateTime.Now:HH:mm:ss} | Tìm thấy {summary.TotalOrders:N0} hóa đơn | Doanh thu: {summary.TotalRevenue:N0} đ";
                    lblStatus.ForeColor = Color.DimGray;
                }
                else
                {
                    lblStatus.Text = "Không có dữ liệu báo cáo trong khoảng thời gian này.";
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
            // Tạo bảng HTML mở được trực tiếp trên Excel có định dạng đẹp mắt
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
}
