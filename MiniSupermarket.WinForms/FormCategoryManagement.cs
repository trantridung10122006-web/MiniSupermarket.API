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
    public partial class FormCategoryManagement : Form
    {
        private const string SearchPlaceholder = "Nhập từ khóa...";

        // The shared ApiClientService points to the API development endpoint.
        private const string Endpoint = "Categories";

        // =====================================================
        // DANH SÁCH NHÓM HÀNG (bản sao dữ liệu lấy từ DB)
        // =====================================================

        private List<Category> categories =
            new List<Category>();


        // =====================================================
        // CONSTRUCTOR
        // =====================================================

        public FormCategoryManagement()
        {
            InitializeComponent();

            // Đi bằng phím mũi tên cũng cập nhật ô thông tin bên phải
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Nhấn Enter ở ô tìm kiếm = bấm nút Tìm kiếm
            txtSearch.KeyDown += txtSearch_KeyDown;

            SetPlaceholder();
            WinFormsTheme.Apply(this);
        }


        // =====================================================
        // FORM LOAD
        // =====================================================

        private async void FormCategoryManagement_Load(
            object sender,
            EventArgs e)
        {
            await LoadData();

            SelectFirstRow();
        }


        // =====================================================
        // TẢI DỮ LIỆU THẬT TỪ DB (QUA API)
        // =====================================================

        private async Task<bool> LoadData()
        {
            SetStatus("Đang tải dữ liệu...");

            try
            {
                var data = await ApiClientService.Client
                    .GetFromJsonAsync<List<Category>>(Endpoint);

                categories = data ?? new List<Category>();

                DisplayCategories(categories);

                return true;
            }
            catch (HttpRequestException ex)
            {
                SetStatus("Không thể kết nối API");

                MessageBox.Show(
                    "Không thể kết nối API để tải nhóm hàng.\n" +
                    "Hãy kiểm tra API (MiniSupermarket.API) đang chạy và " +
                    "địa chỉ trong ApiClientService đúng cổng.\n\n" +
                    ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                SetStatus("Hết thời gian chờ");

                MessageBox.Show(
                    "Yêu cầu đã hết thời gian chờ. Vui lòng thử lại!",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                SetStatus("Lỗi tải dữ liệu");

                MessageBox.Show(
                    ex.Message,
                    "Lỗi tải dữ liệu nhóm hàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return false;
        }


        // =====================================================
        // HIỂN THỊ DANH SÁCH
        // =====================================================

        private void DisplayCategories(
            List<Category> list)
        {
            dataGridView1.Rows.Clear();

            foreach (Category category in list)
            {
                dataGridView1.Rows.Add(
                    category.CategoryId,
                    category.CategoryName,
                    category.Description);
            }

            SetStatus("Có " + list.Count + " nhóm hàng");
        }


        // =====================================================
        // CHỌN DÒNG / HIỂN THỊ CHI TIẾT
        // =====================================================

        private void dataGridView1_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            ShowCategory(e.RowIndex);
        }

        private void dataGridView1_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (dataGridView1.CurrentRow != null &&
                dataGridView1.CurrentRow.Selected)
            {
                ShowCategory(dataGridView1.CurrentRow.Index);
            }
        }

        private void ShowCategory(
            int rowIndex)
        {
            if (rowIndex < 0 ||
                rowIndex >= dataGridView1.Rows.Count)
            {
                return;
            }

            DataGridViewRow row =
                dataGridView1.Rows[rowIndex];

            txtID.Text =
                Convert.ToString(
                    row.Cells[0].Value);

            txtName.Text =
                Convert.ToString(
                    row.Cells[1].Value);

            txtDescription.Text =
                Convert.ToString(
                    row.Cells[2].Value);
        }

        private void SelectFirstRow()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.ClearSelection();

                dataGridView1.Rows[0].Selected =
                    true;

                dataGridView1.CurrentCell =
                    dataGridView1.Rows[0].Cells[0];

                ShowCategory(0);
            }
            else
            {
                ClearInput();
            }
        }

        private void SelectCategoryByID(
            int id)
        {
            for (int i = 0;
                 i < dataGridView1.Rows.Count;
                 i++)
            {
                string currentID =
                    Convert.ToString(
                        dataGridView1.Rows[i]
                            .Cells[0].Value);

                if (currentID ==
                    id.ToString())
                {
                    dataGridView1.ClearSelection();

                    dataGridView1.Rows[i].Selected =
                        true;

                    dataGridView1.CurrentCell =
                        dataGridView1.Rows[i].Cells[0];

                    ShowCategory(i);

                    return;
                }
            }
        }

        private void ClearInput()
        {
            txtID.Clear();
            txtName.Clear();
            txtDescription.Clear();
        }


        // =====================================================
        // TÌM KIẾM (lọc trên danh sách đã tải)
        // =====================================================

        private void txtSearch_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            btnSearch.PerformClick();
        }

        private void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            string keyword =
                txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword) ||
                keyword == SearchPlaceholder)
            {
                DisplayCategories(categories);

                SelectFirstRow();

                return;
            }

            // (?? "") để không bị lỗi khi Mô tả trong DB là NULL
            List<Category> result =
                categories
                    .Where(x =>
                        (x.CategoryName ?? "").IndexOf(
                            keyword,
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0

                        ||

                        (x.Description ?? "").IndexOf(
                            keyword,
                            StringComparison.OrdinalIgnoreCase
                        ) >= 0
                    )
                    .ToList();

            DisplayCategories(result);

            if (result.Count > 0)
            {
                SelectFirstRow();

                SetStatus("Tìm thấy " + result.Count + " nhóm hàng");
            }
            else
            {
                ClearInput();

                SetStatus("Không tìm thấy");

                MessageBox.Show(
                    "Không tìm thấy nhóm hàng!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }


        // =====================================================
        // TẢI LẠI (lấy lại dữ liệu mới nhất từ DB)
        // =====================================================

        private async void btnReload_Click(
            object sender,
            EventArgs e)
        {
            SetPlaceholder();

            SetBusy(true);

            try
            {
                if (await LoadData())
                {
                    SelectFirstRow();

                    SetStatus("Đã tải lại dữ liệu");
                }
            }
            finally
            {
                SetBusy(false);
            }
        }


        // =====================================================
        // THÊM MỚI (lưu vào DB)
        // =====================================================

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (!ValidateName(name, null))
                return;

            var request = new
            {
                CategoryName = name,
                Description = description
            };

            SetBusy(true);

            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client
                        .PostAsJsonAsync(Endpoint, request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(
                        response,
                        "Thêm nhóm hàng thất bại!");

                    return;
                }

                // Nếu API trả về đối tượng vừa tạo thì lấy ID để chọn lại dòng đó
                int? newId = null;

                try
                {
                    Category created =
                        await response.Content.ReadFromJsonAsync<Category>();

                    if (created != null && created.CategoryId > 0)
                        newId = created.CategoryId;
                }
                catch
                {
                    // API không trả nội dung → bỏ qua, tìm theo tên bên dưới
                }

                await LoadData();

                if (newId == null)
                {
                    newId = categories
                        .Where(x => string.Equals(
                            x.CategoryName, name,
                            StringComparison.OrdinalIgnoreCase))
                        .Select(x => (int?)x.CategoryId)
                        .FirstOrDefault();
                }

                if (newId.HasValue)
                    SelectCategoryByID(newId.Value);

                SetStatus("Đã thêm nhóm hàng");

                MessageBox.Show(
                    "Thêm mới thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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
        // CẬP NHẬT (lưu vào DB)
        // =====================================================

        private async void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            int id;

            if (!int.TryParse(
                txtID.Text,
                out id))
            {
                MessageBox.Show(
                    "Vui lòng chọn nhóm hàng cần cập nhật!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (!ValidateName(name, id))
                return;

            var request = new
            {
                CategoryId = id,
                CategoryName = name,
                Description = description
            };

            SetBusy(true);

            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client
                        .PutAsJsonAsync($"{Endpoint}/{id}", request);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(
                        response,
                        "Cập nhật nhóm hàng thất bại!");

                    return;
                }

                await LoadData();

                SelectCategoryByID(id);

                SetStatus("Đã cập nhật nhóm hàng");

                MessageBox.Show(
                    "Cập nhật thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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
        // XÓA (xóa khỏi DB)
        // =====================================================

        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            int id;

            if (!int.TryParse(
                txtID.Text,
                out id))
            {
                MessageBox.Show(
                    "Vui lòng chọn nhóm hàng cần xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string displayName =
                categories
                    .Where(x => x.CategoryId == id)
                    .Select(x => x.CategoryName)
                    .FirstOrDefault() ?? txtName.Text;

            DialogResult result =
                MessageBox.Show(
                    "Bạn có chắc muốn xóa nhóm hàng \""
                    + displayName
                    + "\" không?",

                    "Xác nhận xóa",

                    MessageBoxButtons.YesNo,

                    MessageBoxIcon.Question
                );

            if (result != DialogResult.Yes)
                return;

            SetBusy(true);

            try
            {
                HttpResponseMessage response =
                    await ApiClientService.Client
                        .DeleteAsync($"{Endpoint}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    await ShowApiErrorAsync(
                        response,
                        "Không thể xóa nhóm hàng!\n" +
                        "(Có thể nhóm hàng đang có sản phẩm sử dụng.)");

                    return;
                }

                await LoadData();

                SelectFirstRow();

                SetStatus("Đã xóa nhóm hàng");

                MessageBox.Show(
                    "Xóa thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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
        // HÀM HỖ TRỢ
        // =====================================================

        private bool ValidateName(
            string name,
            int? currentId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Vui lòng nhập Tên Nhóm hàng!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtName.Focus();

                return false;
            }

            bool duplicate =
                categories.Any(x =>
                    string.Equals(
                        x.CategoryName?.Trim(),
                        name,
                        StringComparison.OrdinalIgnoreCase)
                    && x.CategoryId != currentId);

            if (duplicate)
            {
                MessageBox.Show(
                    "Tên nhóm hàng đã tồn tại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtName.Focus();
                txtName.SelectAll();

                return false;
            }

            return true;
        }

        private void SetBusy(bool busy)
        {
            UseWaitCursor = busy;

            btnAdd.Enabled = !busy;
            btnUpdate.Enabled = !busy;
            btnDelete.Enabled = !busy;
            btnReload.Enabled = !busy;
            btnSearch.Enabled = !busy;
        }

        private void SetStatus(string message)
        {
            lblStatus.Text = message;
        }

        private void SetPlaceholder()
        {
            txtSearch.Text = SearchPlaceholder;
            txtSearch.ForeColor = Color.Gray;
        }

        private static void ShowConnectionError(Exception ex)
        {
            string message;

            if (ex is HttpRequestException)
                message = "Không thể kết nối máy chủ.\n\n" + ex.Message;
            else if (ex is TaskCanceledException)
                message = "Yêu cầu đã hết thời gian chờ!";
            else
                message = "Đã xảy ra lỗi.\n\n" + ex.Message;

            MessageBox.Show(
                message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static async Task ShowApiErrorAsync(
            HttpResponseMessage response,
            string headline)
        {
            string detail =
                await response.Content.ReadAsStringAsync();

            MessageBox.Show(
                headline + "\n" +
                "Mã trạng thái: " + (int)response.StatusCode +
                " (" + response.StatusCode + ")\n" +
                "Chi tiết: " + detail,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }


        // =====================================================
        // PLACEHOLDER - ENTER
        // =====================================================

        private void txtSearch_Enter(
            object sender,
            EventArgs e)
        {
            if (txtSearch.Text ==
                SearchPlaceholder)
            {
                txtSearch.Clear();

                txtSearch.ForeColor =
                    Color.Black;
            }
        }


        // =====================================================
        // PLACEHOLDER - LEAVE
        // =====================================================

        private void txtSearch_Leave(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtSearch.Text))
            {
                SetPlaceholder();
            }
        }


        // =====================================================
        // MODEL CATEGORY
        // =====================================================

        private class Category
        {
            public int CategoryId { get; set; }

            public string CategoryName { get; set; }

            public string Description { get; set; }
        }
    }
}