using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCustomer : Form
    {
        private List<CustomerDto> _allCustomers = new List<CustomerDto>();

        public FormCustomer()
        {
            InitializeComponent();
            SetupGrid();
            WireEvents();
            UpdateButtonState();
        }

        // =====================================================
        // 1. CẤU HÌNH BẢNG + SỰ KIỆN
        // =====================================================
        private void SetupGrid()
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;

            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.BorderStyle = BorderStyle.None;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCustomers.ColumnHeadersHeight = 36;
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.RowTemplate.Height = 32;
            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 250, 249);
            dgvCustomers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgvCustomers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCustomers.GridColor = Color.FromArgb(224, 224, 224);
            dgvCustomers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCustomers.AllowUserToResizeRows = false;

            dgvCustomers.Columns.Clear();
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCustomerId",
                DataPropertyName = nameof(CustomerDto.CustomerId),
                HeaderText = "Mã",
                Width = 60
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFullName",
                DataPropertyName = nameof(CustomerDto.FullName),
                HeaderText = "Họ và tên",
                Width = 180
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhone",
                DataPropertyName = nameof(CustomerDto.Phone),
                HeaderText = "Số điện thoại",
                Width = 120
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEmail",
                DataPropertyName = nameof(CustomerDto.Email),
                HeaderText = "Email",
                Width = 170
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAddress",
                DataPropertyName = nameof(CustomerDto.Address),
                HeaderText = "Địa chỉ",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPoints",
                DataPropertyName = nameof(CustomerDto.Points),
                HeaderText = "Điểm",
                Width = 80,
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
            Load += FormCustomer_Load;
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            btnReload.Click += async (s, e) => await LoadCustomersAsync();
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += (s, e) => ClearInput();

            // Ô số điện thoại chỉ nhận chữ số
            txtPhone.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };
        }

        private async void FormCustomer_Load(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
            txtFullName.Focus();
        }

        // =====================================================
        // 2. TẢI DANH SÁCH TỪ DB (QUA API)
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
                ClearInput();
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow?.DataBoundItem is CustomerDto c && dgvCustomers.CurrentRow.Selected)
            {
                txtCustomerId.Text = c.CustomerId.ToString();
                txtFullName.Text = c.FullName;
                txtPhone.Text = c.Phone;
                txtEmail.Text = c.Email ?? string.Empty;
                txtAddress.Text = c.Address ?? string.Empty;
                txtPoints.Text = c.Points.ToString("N0");
            }

            UpdateButtonState();
        }

        // =====================================================
        // 3. THÊM / SỬA / XÓA
        // =====================================================
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(null)) return;

            var request = new
            {
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Points = 0
            };

            SetBusy(true);
            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client.PostAsJsonAsync("customers", request);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearInput();
                    await LoadCustomersAsync();
                }
                else
                {
                    await ShowApiErrorAsync(response, "Thêm khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedId(out int id))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(id)) return;

            // Giữ nguyên điểm tích lũy hiện có
            int points = _allCustomers.FirstOrDefault(c => c.CustomerId == id)?.Points ?? 0;

            var request = new
            {
                CustomerId = id,
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Points = points
            };

            SetBusy(true);
            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client.PutAsJsonAsync($"customers/{id}", request);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadCustomersAsync(id);
                }
                else
                {
                    await ShowApiErrorAsync(response, "Cập nhật khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedId(out int id))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa khách hàng \"{txtFullName.Text}\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            SetBusy(true);
            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client.DeleteAsync($"customers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đã xóa khách hàng!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearInput();
                    await LoadCustomersAsync();
                }
                else
                {
                    await ShowApiErrorAsync(response,
                        "Không thể xóa khách hàng!\n(Có thể khách đã có đơn hàng trong hệ thống.)");
                }
            }
            catch (Exception ex)
            {
                ShowConnectionError(ex);
            }
            finally
            {
                SetBusy(false);
            }
        }

        // =====================================================
        // 4. HÀM HỖ TRỢ
        // =====================================================
        private bool ValidateInput(int? currentId)
        {
            string name = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (name.Length == 0)
            {
                Warn("Vui lòng nhập họ và tên khách hàng!", txtFullName);
                return false;
            }

            if (phone.Length == 0)
            {
                Warn("Vui lòng nhập số điện thoại!", txtPhone);
                return false;
            }

            if (!Regex.IsMatch(phone, @"^0\d{9,10}$"))
            {
                Warn("Số điện thoại không hợp lệ!\n(Phải bắt đầu bằng 0 và có 10–11 chữ số.)", txtPhone);
                return false;
            }

            if (email.Length > 0 && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Warn("Email không hợp lệ!", txtEmail);
                return false;
            }

            bool duplicate = _allCustomers.Any(c =>
                c.Phone?.Trim() == phone && c.CustomerId != currentId);

            if (duplicate)
            {
                Warn("Số điện thoại này đã thuộc về khách hàng khác!", txtPhone);
                return false;
            }

            return true;
        }

        private static void Warn(string message, TextBox focusTarget)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focusTarget.Focus();
            focusTarget.SelectAll();
        }

        private bool HasSelection()
        {
            return !string.IsNullOrWhiteSpace(txtCustomerId.Text);
        }

        private bool TryGetSelectedId(out int id)
        {
            return int.TryParse(txtCustomerId.Text, out id) && id > 0;
        }

        private void ClearInput()
        {
            dgvCustomers.ClearSelection();
            txtCustomerId.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtPoints.Clear();
            UpdateButtonState();
            txtFullName.Focus();
        }

        private void UpdateButtonState()
        {
            bool selected = HasSelection();
            btnUpdate.Enabled = selected;
            btnDelete.Enabled = selected;
            btnAdd.Enabled = !selected;

            // Dùng lại tiện ích đổi màu nút đã khai báo trong FormCategory.cs
            btnUpdate.Alpha(selected);
            btnDelete.Alpha(selected);
            btnAdd.Alpha(!selected);
        }

        private void SetBusy(bool busy)
        {
            UseWaitCursor = busy;
            pnlRight.Enabled = !busy;
            if (!busy) UpdateButtonState();
        }

        private static void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void ShowConnectionError(Exception ex)
        {
            if (ex is HttpRequestException)
                ShowError("Không thể kết nối máy chủ.\n\n" + ex.Message, "Lỗi kết nối");
            else if (ex is TaskCanceledException)
                ShowError("Yêu cầu đã hết thời gian chờ!", "Lỗi");
            else
                ShowError("Đã xảy ra lỗi.\n\n" + ex.Message, "Lỗi");
        }

        private static async Task ShowApiErrorAsync(HttpResponseMessage response, string headline)
        {
            string detail = await response.Content.ReadAsStringAsync();

            ShowError(
                $"{headline}\n" +
                $"Mã trạng thái: {(int)response.StatusCode} ({response.StatusCode})\n" +
                $"Chi tiết: {detail}",
                "Lỗi");
        }
    }
}