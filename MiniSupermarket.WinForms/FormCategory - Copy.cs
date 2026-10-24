using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCategory : Form
    {
        private List<CategoryDto> _allCategories = new List<CategoryDto>();

        public FormCategory()
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
            dgvCategories.AutoGenerateColumns = false;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.ReadOnly = true;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.MultiSelect = false;

            // Giao diện bảng
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.None;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.EnableHeadersVisualStyles = false;
            dgvCategories.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
            dgvCategories.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategories.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCategories.ColumnHeadersHeight = 36;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCategories.RowTemplate.Height = 32;
            dgvCategories.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 250, 249);
            dgvCategories.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgvCategories.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCategories.GridColor = Color.FromArgb(224, 224, 224);
            dgvCategories.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCategories.AllowUserToResizeRows = false;

            dgvCategories.Columns.Clear();
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryId",
                DataPropertyName = nameof(CategoryDto.CategoryId),
                HeaderText = "Mã",
                Width = 80
            });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategoryName",
                DataPropertyName = nameof(CategoryDto.CategoryName),
                HeaderText = "Tên danh mục",
                Width = 220
            });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDescription",
                DataPropertyName = nameof(CategoryDto.Description),
                HeaderText = "Mô tả",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void WireEvents()
        {
            Load += FormCategory_Load;
            dgvCategories.SelectionChanged += dgvCategories_SelectionChanged;
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            btnReload.Click += async (s, e) => await LoadCategoriesAsync();
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += (s, e) => ClearInput();
            txtCategoryName.KeyDown += txtCategoryName_KeyDown;
        }

        private async void FormCategory_Load(object sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            txtCategoryName.Focus();
        }

        // =====================================================
        // 2. TẢI DANH SÁCH TỪ DB (QUA API)
        // =====================================================
        private async Task LoadCategoriesAsync(int? reselectId = null)
        {
            try
            {
                btnReload.Enabled = false;

                var list = await ApiClientService.Client
                    .GetFromJsonAsync<List<CategoryDto>>("categories");

                _allCategories = list ?? new List<CategoryDto>();
                ApplyFilter(reselectId);
            }
            catch (HttpRequestException ex)
            {
                ShowError("Không thể kết nối API để tải danh mục.\n\n" + ex.Message, "Lỗi kết nối");
            }
            catch (TaskCanceledException)
            {
                ShowError("Yêu cầu đã hết thời gian chờ. Vui lòng thử lại!", "Lỗi kết nối");
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi tải danh mục.\n\n" + ex.Message, "Lỗi");
            }
            finally
            {
                btnReload.Enabled = true;
            }
        }

        private void ApplyFilter(int? reselectId = null)
        {
            string keyword = (txtSearch.Text ?? string.Empty).Trim();

            IEnumerable<CategoryDto> query = _allCategories;
            if (keyword.Length > 0)
            {
                query = query.Where(c =>
                    (c.CategoryName ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (c.Description ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.CategoryId.ToString() == keyword);
            }

            var result = query.ToList();
            dgvCategories.DataSource = result;
            lblCount.Text = $"{result.Count} danh mục";

            if (reselectId.HasValue)
            {
                foreach (DataGridViewRow row in dgvCategories.Rows)
                {
                    if (row.DataBoundItem is CategoryDto c && c.CategoryId == reselectId.Value)
                    {
                        dgvCategories.ClearSelection();
                        row.Selected = true;
                        dgvCategories.CurrentCell = row.Cells[0];
                        return;
                    }
                }
            }

            if (result.Count == 0)
                ClearInput();
        }

        private void dgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow?.DataBoundItem is CategoryDto c && dgvCategories.CurrentRow.Selected)
            {
                txtCategoryId.Text = c.CategoryId.ToString();
                txtCategoryName.Text = c.CategoryName;
                txtDescription.Text = c.Description ?? string.Empty;
            }

            UpdateButtonState();
        }

        // =====================================================
        // 3. THÊM / SỬA / XÓA
        // =====================================================
        private void txtCategoryName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            if (HasSelection()) btnUpdate.PerformClick();
            else btnAdd.PerformClick();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(null)) return;

            var request = new
            {
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            SetBusy(true);
            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client.PostAsJsonAsync("categories", request);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm danh mục thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearInput();
                    await LoadCategoriesAsync();
                }
                else
                {
                    await ShowApiErrorAsync(response, "Thêm danh mục thất bại!");
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
                MessageBox.Show("Vui lòng chọn danh mục cần cập nhật!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(id)) return;

            var request = new
            {
                CategoryId = id,
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            SetBusy(true);
            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client.PutAsJsonAsync($"categories/{id}", request);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật danh mục thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadCategoriesAsync(id);
                }
                else
                {
                    await ShowApiErrorAsync(response, "Cập nhật danh mục thất bại!");
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
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa danh mục \"{txtCategoryName.Text}\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            SetBusy(true);
            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client.DeleteAsync($"categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đã xóa danh mục!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearInput();
                    await LoadCategoriesAsync();
                }
                else
                {
                    await ShowApiErrorAsync(response,
                        "Không thể xóa danh mục!\n(Có thể danh mục đang có sản phẩm sử dụng.)");
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
            string name = txtCategoryName.Text.Trim();

            if (name.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return false;
            }

            bool duplicate = _allCategories.Any(c =>
                string.Equals(c.CategoryName?.Trim(), name, StringComparison.OrdinalIgnoreCase) &&
                c.CategoryId != currentId);

            if (duplicate)
            {
                MessageBox.Show("Tên danh mục đã tồn tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                txtCategoryName.SelectAll();
                return false;
            }

            return true;
        }

        private bool HasSelection()
        {
            return !string.IsNullOrWhiteSpace(txtCategoryId.Text);
        }

        private bool TryGetSelectedId(out int id)
        {
            return int.TryParse(txtCategoryId.Text, out id) && id > 0;
        }

        private void ClearInput()
        {
            dgvCategories.ClearSelection();
            txtCategoryId.Clear();
            txtCategoryName.Clear();
            txtDescription.Clear();
            UpdateButtonState();
            txtCategoryName.Focus();
        }

        private void UpdateButtonState()
        {
            bool selected = HasSelection();
            btnUpdate.Enabled = selected;
            btnDelete.Enabled = selected;
            btnAdd.Enabled = !selected;

            // Làm mờ nút bị vô hiệu hóa cho dễ nhìn
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

    // =========================================================
    // TIỆN ÍCH: làm mờ nút khi bị vô hiệu hóa
    // =========================================================
    internal static class ButtonExtensions
    {
        public static void Alpha(this Button button, bool enabled)
        {
            // Nút Flat không tự đổi màu khi Disabled nên đổi thủ công
            if (button.Tag == null)
                button.Tag = button.BackColor;

            Color original = (Color)button.Tag;
            button.BackColor = enabled ? original : Color.FromArgb(189, 189, 189);
        }
    }

    // =========================================================
    // DTO DANH MỤC API TRẢ VỀ
    // =========================================================
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}