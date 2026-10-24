using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCustomerManagement : Form
    {
        private List<CustomerDto> _allCustomers = new List<CustomerDto>();

        // Dùng để bỏ qua kết quả cũ khi người dùng chọn khách khác quá nhanh
        private int _orderLoadVersion;

        public FormCustomerManagement()
        {
            InitializeComponent();
            SetupCustomerGrid();
            SetupOrderGrid();
            WireEvents();
            ShowEmptyDetail();
        }

        // =====================================================
        // 1. CẤU HÌNH BẢNG + SỰ KIỆN
        // =====================================================
        private static void StyleGrid(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.RowTemplate.Height = 32;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 250, 249);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.GridColor = Color.FromArgb(224, 224, 224);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.AllowUserToResizeRows = false;
            dgv.Columns.Clear();
        }

        private void SetupCustomerGrid()
        {
            StyleGrid(dgvCustomers);

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(CustomerDto.CustomerId),
                HeaderText = "Mã",
                Width = 60
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(CustomerDto.FullName),
                HeaderText = "Họ và tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(CustomerDto.Phone),
                HeaderText = "Số điện thoại",
                Width = 130
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(CustomerDto.Points),
                HeaderText = "Điểm",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
        }

        private void SetupOrderGrid()
        {
            StyleGrid(dgvOrders);

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderHistoryDto.OrderId),
                HeaderText = "Mã đơn",
                Width = 80
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderHistoryDto.OrderDate),
                HeaderText = "Ngày giờ",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderHistoryDto.CashierUsername),
                HeaderText = "Thu ngân",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderHistoryDto.ItemCount),
                HeaderText = "Số mặt hàng",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderHistoryDto.TotalAmount),
                HeaderText = "Tổng tiền",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                }
            });
        }

        private void WireEvents()
        {
            Load += FormCustomerManagement_Load;
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            btnReload.Click += async (s, e) => await LoadCustomersAsync(GetSelectedCustomer()?.CustomerId);
            btnOpenEdit.Click += btnOpenEdit_Click;
        }

        private async void FormCustomerManagement_Load(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        // =====================================================
        // 2. TẢI DANH SÁCH KHÁCH HÀNG TỪ DB (QUA API)
        // =====================================================
        private async Task LoadCustomersAsync(int? reselectId = null)
        {
            try
            {
                btnReload.Enabled = false;

                var list = await ApiClientService.Client
                    .GetFromJsonAsync<List<CustomerDto>>("customers");

                _allCustomers = list ?? new List<CustomerDto>();
                ApplyFilter(reselectId);
            }
            catch (HttpRequestException ex)
            {
                ShowError("Không thể kết nối API để tải khách hàng.\n\n" + ex.Message, "Lỗi kết nối");
            }
            catch (TaskCanceledException)
            {
                ShowError("Yêu cầu đã hết thời gian chờ. Vui lòng thử lại!", "Lỗi kết nối");
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi tải khách hàng.\n\n" + ex.Message, "Lỗi");
            }
            finally
            {
                btnReload.Enabled = true;
            }
        }

        private void ApplyFilter(int? reselectId = null)
        {
            string keyword = (txtSearch.Text ?? string.Empty).Trim();

            IEnumerable<CustomerDto> query = _allCustomers;
            if (keyword.Length > 0)
            {
                query = query.Where(c =>
                    (c.FullName ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (c.Phone ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (c.Email ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            var result = query.ToList();
            dgvCustomers.DataSource = result;
            lblCount.Text = $"{result.Count} khách hàng";

            if (reselectId.HasValue)
            {
                foreach (DataGridViewRow row in dgvCustomers.Rows)
                {
                    if (row.DataBoundItem is CustomerDto c && c.CustomerId == reselectId.Value)
                    {
                        dgvCustomers.ClearSelection();
                        row.Selected = true;
                        dgvCustomers.CurrentCell = row.Cells[0];
                        return;
                    }
                }
            }

            if (result.Count == 0)
                ShowEmptyDetail();
        }

        private CustomerDto GetSelectedCustomer()
        {
            if (dgvCustomers.CurrentRow != null && dgvCustomers.CurrentRow.Selected)
                return dgvCustomers.CurrentRow.DataBoundItem as CustomerDto;

            return null;
        }

        private async void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            CustomerDto customer = GetSelectedCustomer();

            if (customer == null)
            {
                ShowEmptyDetail();
                return;
            }

            ShowCustomerDetail(customer);
            await LoadOrderHistoryAsync(customer);
        }

        // =====================================================
        // 3. HIỂN THỊ CHI TIẾT + LỊCH SỬ MUA HÀNG
        // =====================================================
        private void ShowEmptyDetail()
        {
            _orderLoadVersion++;

            lblName.Text = "Chọn một khách hàng";
            lblPhone.Text = string.Empty;
            lblEmail.Text = string.Empty;
            lblAddress.Text = string.Empty;

            lblStatOrdersValue.Text = "0";
            lblStatSpentValue.Text = "0 đ";
            lblStatPointsValue.Text = "0";

            dgvOrders.DataSource = null;
            lblHistoryTitle.Text = "LỊCH SỬ MUA HÀNG";
            btnOpenEdit.Enabled = true;
        }

        private void ShowCustomerDetail(CustomerDto c)
        {
            lblName.Text = c.FullName;
            lblPhone.Text = "📞  " + (string.IsNullOrWhiteSpace(c.Phone) ? "—" : c.Phone);
            lblEmail.Text = "✉  " + (string.IsNullOrWhiteSpace(c.Email) ? "—" : c.Email);
            lblAddress.Text = "📍  " + (string.IsNullOrWhiteSpace(c.Address) ? "—" : c.Address);

            lblStatPointsValue.Text = c.Points.ToString("N0");
            lblStatOrdersValue.Text = "...";
            lblStatSpentValue.Text = "...";
            lblHistoryTitle.Text = "LỊCH SỬ MUA HÀNG (đang tải...)";
        }

        private async Task LoadOrderHistoryAsync(CustomerDto customer)
        {
            int version = ++_orderLoadVersion;

            try
            {
                HttpResponseMessage response = await ApiClientService.Client
                    .GetAsync($"customers/{customer.CustomerId}/orders");

                // Người dùng đã chọn khách khác trong lúc chờ
                if (version != _orderLoadVersion) return;

                List<OrderHistoryDto> orders;

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    orders = new List<OrderHistoryDto>();
                }
                else if (!response.IsSuccessStatusCode)
                {
                    ShowOrderLoadFailed($"Không tải được lịch sử (mã lỗi {(int)response.StatusCode}).");
                    return;
                }
                else
                {
                    orders = await response.Content.ReadFromJsonAsync<List<OrderHistoryDto>>()
                             ?? new List<OrderHistoryDto>();

                    if (version != _orderLoadVersion) return;
                }

                orders = orders.OrderByDescending(o => o.OrderDate).ToList();
                dgvOrders.DataSource = orders;

                lblStatOrdersValue.Text = orders.Count.ToString("N0");
                lblStatSpentValue.Text = $"{orders.Sum(o => o.TotalAmount):N0} đ";
                lblHistoryTitle.Text = orders.Count == 0
                    ? "LỊCH SỬ MUA HÀNG (chưa có đơn nào)"
                    : $"LỊCH SỬ MUA HÀNG ({orders.Count} đơn, gần nhất: {orders[0].OrderDate:dd/MM/yyyy})";
            }
            catch (Exception ex)
            {
                if (version != _orderLoadVersion) return;
                ShowOrderLoadFailed("Lỗi kết nối: " + ex.Message);
            }
        }

        private void ShowOrderLoadFailed(string message)
        {
            dgvOrders.DataSource = null;
            lblStatOrdersValue.Text = "—";
            lblStatSpentValue.Text = "—";
            lblHistoryTitle.Text = "LỊCH SỬ MUA HÀNG — " + message;
        }

        // =====================================================
        // 4. MỞ FORM THÊM / SỬA / XÓA
        // =====================================================
        private async void btnOpenEdit_Click(object sender, EventArgs e)
        {
            int? currentId = GetSelectedCustomer()?.CustomerId;

            using (var form = new FormCustomer())
            {
                form.ShowDialog(this);
            }

            // Tải lại để phản ánh thay đổi vừa thực hiện
            await LoadCustomersAsync(currentId);
        }

        private static void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // =========================================================
    // DTO LỊCH SỬ ĐƠN HÀNG API TRẢ VỀ
    // =========================================================
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string CashierUsername { get; set; } = string.Empty;

        public int ItemCount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}