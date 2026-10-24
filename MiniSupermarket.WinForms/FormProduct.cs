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
    public partial class FormProduct : Form
    {
        private List<ProductApiDto> _products = new List<ProductApiDto>();
        private bool _loadingCategories;

        public FormProduct()
        {
            InitializeComponent();

            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            nudPrice.Minimum = 0;
            nudPrice.Maximum = 1000000000;
            nudPrice.DecimalPlaces = 0;
            nudPrice.ThousandsSeparator = true;

            nudStock.Minimum = 0;
            nudStock.Maximum = 1000000;

            Load += FormProductManagement_Load;
            dgvProducts.CellClick += dgvProducts_CellClick;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += (s, e) => ClearInputs();
            btnRefresh.Click += async (s, e) => await RefreshDataAsync();
            WinFormsTheme.Apply(this);
        }

        private async void FormProductManagement_Load(object sender, EventArgs e)
        {
            await RefreshDataAsync();
        }

        // Tải danh mục và sản phẩm
        private async Task RefreshDataAsync()
        {
            btnRefresh.Enabled = false;

            try
            {
                await LoadCategoriesToComboAsync();
                await LoadProductsAsync();
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private async Task LoadCategoriesToComboAsync()
        {
            _loadingCategories = true;

            try
            {
                var categories =
                    await ApiClientService.Client.GetFromJsonAsync<List<CategoryApiDto>>(
                        "categories");

                cboCategory.DataSource = null;
                cboCategory.DisplayMember = nameof(CategoryApiDto.CategoryName);
                cboCategory.ValueMember = nameof(CategoryApiDto.CategoryId);
                cboCategory.DataSource = categories;

                if (categories.Count > 0)
                    cboCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh mục.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _loadingCategories = false;
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var result =
                    await ApiClientService.Client.GetFromJsonAsync<List<ProductApiDto>>(
                        "products");

                _products = result ?? new List<ProductApiDto>();
                dgvProducts.DataSource = null;
                dgvProducts.DataSource = _products.Select(p => new
                {
                    p.ProductId,
                    p.Barcode,
                    p.ProductName,
                    p.Price,
                    p.StockQuantity,
                    CategoryName = !string.IsNullOrEmpty(p.CategoryName) ? p.CategoryName : (cboCategory.DataSource as List<CategoryApiDto>)?.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.CategoryName ?? (p.CategoryId?.ToString() ?? "—"),
                    p.CategoryId
                }).ToList();

                if (dgvProducts.Columns["ProductId"] != null)
                    dgvProducts.Columns["ProductId"].HeaderText = "Mã SP";

                if (dgvProducts.Columns["Barcode"] != null)
                    dgvProducts.Columns["Barcode"].HeaderText = "Mã vạch";

                if (dgvProducts.Columns["ProductName"] != null)
                    dgvProducts.Columns["ProductName"].HeaderText = "Tên sản phẩm";

                if (dgvProducts.Columns["Price"] != null)
                {
                    dgvProducts.Columns["Price"].HeaderText = "Đơn giá";
                    dgvProducts.Columns["Price"].DefaultCellStyle.Format = "N0";
                }

                if (dgvProducts.Columns["StockQuantity"] != null)
                    dgvProducts.Columns["StockQuantity"].HeaderText = "Tồn kho";

                if (dgvProducts.Columns["CategoryName"] != null)
                    dgvProducts.Columns["CategoryName"].HeaderText = "Danh mục";

                if (dgvProducts.Columns["CategoryId"] != null)
                    dgvProducts.Columns["CategoryId"].Visible = false;

                lblStatus.Text = $"Tổng số sản phẩm: {_products.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách sản phẩm.\n" + ex.Message,
                    "Lỗi API",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Chọn dòng sản phẩm
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int id = Convert.ToInt32(
                dgvProducts.Rows[e.RowIndex].Cells["ProductId"].Value);

            var product = _products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return;

            txtId.Text = product.ProductId.ToString();
            txtBarcode.Text = product.Barcode;
            txtProductName.Text = product.ProductName;
            nudPrice.Value = Math.Min(
                nudPrice.Maximum, Math.Max(nudPrice.Minimum, product.Price));
            nudStock.Value = Math.Min(
                nudStock.Maximum, Math.Max(nudStock.Minimum, product.StockQuantity));

            if (product.CategoryId.HasValue)
                cboCategory.SelectedValue = product.CategoryId.Value;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã vạch.");
                txtBarcode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.");
                txtProductName.Focus();
                return false;
            }

            if (cboCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục.");
                cboCategory.Focus();
                return false;
            }

            return true;
        }

        private object BuildProductRequest()
        {
            return new
            {
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Price = nudPrice.Value,
                StockQuantity = (int)nudStock.Value,
                CategoryId = Convert.ToInt32(cboCategory.SelectedValue)
            };
        }

        // Thêm sản phẩm
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            btnAdd.Enabled = false;

            try
            {
                 HttpResponseMessage response =
                    await ApiClientService.Client.PostAsJsonAsync(
                        "products", BuildProductRequest());

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm sản phẩm thành công!");
                    await LoadProductsAsync();
                    ClearInputs();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Thêm thất bại ({(int)response.StatusCode}).\n{error}");
                }
            }
            catch (Exception ex)
            {
                ShowApiError(ex);
            }
            finally
            {
                btnAdd.Enabled = true;
            }
        }

        // Cập nhật sản phẩm
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.");
                return;
            }

            if (!ValidateInputs())
                return;

            btnUpdate.Enabled = false;

            try
            {
                var request = new
                {
                    ProductId = id,
                    Barcode = txtBarcode.Text.Trim(),
                    ProductName = txtProductName.Text.Trim(),
                    Price = nudPrice.Value,
                    StockQuantity = (int)nudStock.Value,
                    CategoryId = Convert.ToInt32(cboCategory.SelectedValue)
                };

                HttpResponseMessage response =
                    await ApiClientService.Client.PutAsJsonAsync(
                        $"products/{id}", request);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                    await LoadProductsAsync();
                    ClearInputs();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Cập nhật thất bại ({(int)response.StatusCode}).\n{error}");
                }
            }
            catch (Exception ex)
            {
                ShowApiError(ex);
            }
            finally
            {
                btnUpdate.Enabled = true;
            }
        }

        // Xóa sản phẩm
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm ID = {id}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            btnDelete.Enabled = false;

            try
            {
                 HttpResponseMessage response =
                    await ApiClientService.Client.DeleteAsync($"products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa sản phẩm thành công!");
                    await LoadProductsAsync();
                    ClearInputs();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Xóa thất bại ({(int)response.StatusCode}).\n{error}");
                }
            }
            catch (Exception ex)
            {
                ShowApiError(ex);
            }
            finally
            {
                btnDelete.Enabled = true;
            }
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtBarcode.Clear();
            txtProductName.Clear();
            nudPrice.Value = 0;
            nudStock.Value = 0;

            if (cboCategory.Items.Count > 0)
                cboCategory.SelectedIndex = 0;

            dgvProducts.ClearSelection();
            txtBarcode.Focus();
        }

        private void ShowApiError(Exception ex)
        {
            MessageBox.Show(
                "Đã xảy ra lỗi khi gọi API.\n" + ex.Message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    // DTO riêng cho FormProduct, tránh trùng tên ProductDto ở FormPOS
    public class ProductApiDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int? CategoryId { get; set; }
        public CategoryApiDto Category { get; set; }
        public string CategoryName => Category != null ? Category.CategoryName : string.Empty;
    }

    public class CategoryApiDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
