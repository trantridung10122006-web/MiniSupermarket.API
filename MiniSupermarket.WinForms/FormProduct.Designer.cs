
namespace MiniSupermarket.WinForms
{
    partial class FormProduct
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.TextBox txtProductName;

        private System.Windows.Forms.NumericUpDown nudPrice;
        private System.Windows.Forms.NumericUpDown nudStock;
        private System.Windows.Forms.ComboBox cboCategory;

        private System.Windows.Forms.DataGridView dgvProducts;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblId = new System.Windows.Forms.Label();
            lblBarcode = new System.Windows.Forms.Label();
            lblProductName = new System.Windows.Forms.Label();
            lblPrice = new System.Windows.Forms.Label();
            lblStock = new System.Windows.Forms.Label();
            lblCategory = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();

            txtId = new System.Windows.Forms.TextBox();
            txtBarcode = new System.Windows.Forms.TextBox();
            txtProductName = new System.Windows.Forms.TextBox();

            nudPrice = new System.Windows.Forms.NumericUpDown();
            nudStock = new System.Windows.Forms.NumericUpDown();
            cboCategory = new System.Windows.Forms.ComboBox();

            dgvProducts = new System.Windows.Forms.DataGridView();

            btnAdd = new System.Windows.Forms.Button();
            btnUpdate = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)nudPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();

            SuspendLayout();

            // Form
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1050, 680);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "QUẢN LÝ SẢN PHẨM";
            MinimumSize = new System.Drawing.Size(900, 600);
            BackColor = System.Drawing.Color.White;

            // Tiêu đề
            lblTitle.AutoSize = false;
            lblTitle.Text = "QUẢN LÝ SẢN PHẨM";
            lblTitle.Font = new System.Drawing.Font(
                "Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(35, 75, 130);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitle.Location = new System.Drawing.Point(10, 10);
            lblTitle.Size = new System.Drawing.Size(1020, 45);

            // Mã sản phẩm
            lblId.Text = "Mã sản phẩm:";
            lblId.Location = new System.Drawing.Point(25, 80);
            lblId.Size = new System.Drawing.Size(110, 25);

            txtId.Location = new System.Drawing.Point(140, 76);
            txtId.Size = new System.Drawing.Size(180, 27);
            txtId.ReadOnly = true;

            // Mã vạch
            lblBarcode.Text = "Mã vạch:";
            lblBarcode.Location = new System.Drawing.Point(25, 120);
            lblBarcode.Size = new System.Drawing.Size(110, 25);

            txtBarcode.Location = new System.Drawing.Point(140, 116);
            txtBarcode.Size = new System.Drawing.Size(180, 27);

            // Tên sản phẩm
            lblProductName.Text = "Tên sản phẩm:";
            lblProductName.Location = new System.Drawing.Point(25, 160);
            lblProductName.Size = new System.Drawing.Size(110, 25);

            txtProductName.Location = new System.Drawing.Point(140, 156);
            txtProductName.Size = new System.Drawing.Size(300, 27);

            // Đơn giá
            lblPrice.Text = "Đơn giá:";
            lblPrice.Location = new System.Drawing.Point(490, 80);
            lblPrice.Size = new System.Drawing.Size(100, 25);

            nudPrice.Location = new System.Drawing.Point(600, 76);
            nudPrice.Size = new System.Drawing.Size(180, 27);
            nudPrice.Maximum = 1000000000;
            nudPrice.ThousandsSeparator = true;

            // Tồn kho
            lblStock.Text = "Số lượng tồn:";
            lblStock.Location = new System.Drawing.Point(490, 120);
            lblStock.Size = new System.Drawing.Size(100, 25);

            nudStock.Location = new System.Drawing.Point(600, 116);
            nudStock.Size = new System.Drawing.Size(180, 27);
            nudStock.Maximum = 1000000;

            // Danh mục
            lblCategory.Text = "Danh mục:";
            lblCategory.Location = new System.Drawing.Point(490, 160);
            lblCategory.Size = new System.Drawing.Size(100, 25);

            cboCategory.Location = new System.Drawing.Point(600, 156);
            cboCategory.Size = new System.Drawing.Size(250, 27);
            cboCategory.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Nút chức năng
            btnAdd.Text = "Thêm";
            btnAdd.Location = new System.Drawing.Point(25, 215);
            btnAdd.Size = new System.Drawing.Size(110, 38);
            btnAdd.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            btnUpdate.Text = "Cập nhật";
            btnUpdate.Location = new System.Drawing.Point(145, 215);
            btnUpdate.Size = new System.Drawing.Size(110, 38);
            btnUpdate.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnUpdate.ForeColor = System.Drawing.Color.White;
            btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            btnDelete.Text = "Xóa";
            btnDelete.Location = new System.Drawing.Point(265, 215);
            btnDelete.Size = new System.Drawing.Size(110, 38);
            btnDelete.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            btnDelete.ForeColor = System.Drawing.Color.White;
            btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            btnClear.Text = "Làm mới ô nhập";
            btnClear.Location = new System.Drawing.Point(385, 215);
            btnClear.Size = new System.Drawing.Size(145, 38);

            btnRefresh.Text = "Tải lại dữ liệu";
            btnRefresh.Location = new System.Drawing.Point(540, 215);
            btnRefresh.Size = new System.Drawing.Size(145, 38);

            // Bảng sản phẩm
            dgvProducts.Location = new System.Drawing.Point(25, 275);
            dgvProducts.Size = new System.Drawing.Size(1000, 340);
            dgvProducts.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;
            dgvProducts.BackgroundColor = System.Drawing.Color.White;
            dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // Trạng thái
            lblStatus.Text = "Sẵn sàng";
            lblStatus.Location = new System.Drawing.Point(25, 630);
            lblStatus.Size = new System.Drawing.Size(1000, 25);
            lblStatus.Anchor =
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left;

            // Thêm điều khiển vào Form
            Controls.Add(lblTitle);

            Controls.Add(lblId);
            Controls.Add(txtId);

            Controls.Add(lblBarcode);
            Controls.Add(txtBarcode);

            Controls.Add(lblProductName);
            Controls.Add(txtProductName);

            Controls.Add(lblPrice);
            Controls.Add(nudPrice);

            Controls.Add(lblStock);
            Controls.Add(nudStock);

            Controls.Add(lblCategory);
            Controls.Add(cboCategory);

            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
            Controls.Add(btnRefresh);

            Controls.Add(dgvProducts);
            Controls.Add(lblStatus);

            ((System.ComponentModel.ISupportInitialize)nudPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
