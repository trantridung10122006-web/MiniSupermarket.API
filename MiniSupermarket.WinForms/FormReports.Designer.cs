
namespace MiniSupermarket.WinForms
{
    partial class FormReports
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Label lblRevenueTitle;
        private System.Windows.Forms.Label lblRevenue;
        private System.Windows.Forms.Label lblOrdersTitle;
        private System.Windows.Forms.Label lblOrders;
        private System.Windows.Forms.Label lblProductsTitle;
        private System.Windows.Forms.Label lblProducts;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;

        private System.Windows.Forms.Button btnViewReport;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;

        private System.Windows.Forms.Panel pnlRevenue;
        private System.Windows.Forms.Panel pnlOrders;
        private System.Windows.Forms.Panel pnlProducts;

        private System.Windows.Forms.DataGridView dgvReports;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();

            lblFromDate = new System.Windows.Forms.Label();
            lblToDate = new System.Windows.Forms.Label();

            lblRevenueTitle = new System.Windows.Forms.Label();
            lblRevenue = new System.Windows.Forms.Label();

            lblOrdersTitle = new System.Windows.Forms.Label();
            lblOrders = new System.Windows.Forms.Label();

            lblProductsTitle = new System.Windows.Forms.Label();
            lblProducts = new System.Windows.Forms.Label();

            lblStatus = new System.Windows.Forms.Label();

            dtpFromDate = new System.Windows.Forms.DateTimePicker();
            dtpToDate = new System.Windows.Forms.DateTimePicker();

            btnViewReport = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();

            pnlRevenue = new System.Windows.Forms.Panel();
            pnlOrders = new System.Windows.Forms.Panel();
            pnlProducts = new System.Windows.Forms.Panel();

            dgvReports = new System.Windows.Forms.DataGridView();

            pnlRevenue.SuspendLayout();
            pnlOrders.SuspendLayout();
            pnlProducts.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)dgvReports).BeginInit();

            SuspendLayout();

            // Form
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1100, 720);
            MinimumSize = new System.Drawing.Size(900, 600);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "BÁO CÁO - THỐNG KÊ";
            BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            // Tiêu đề
            lblTitle.Text = "BÁO CÁO - THỐNG KÊ BÁN HÀNG";
            lblTitle.Font = new System.Drawing.Font(
                "Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(35, 75, 130);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.Size = new System.Drawing.Size(1060, 50);
            lblTitle.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            // Ngày bắt đầu
            lblFromDate.Text = "Từ ngày:";
            lblFromDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblFromDate.Location = new System.Drawing.Point(25, 90);
            lblFromDate.Size = new System.Drawing.Size(80, 25);

            dtpFromDate.Format =
                System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFromDate.Location = new System.Drawing.Point(105, 87);
            dtpFromDate.Size = new System.Drawing.Size(160, 27);
            dtpFromDate.Value = System.DateTime.Today.AddDays(-30);

            // Ngày kết thúc
            lblToDate.Text = "Đến ngày:";
            lblToDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblToDate.Location = new System.Drawing.Point(290, 90);
            lblToDate.Size = new System.Drawing.Size(80, 25);

            dtpToDate.Format =
                System.Windows.Forms.DateTimePickerFormat.Short;
            dtpToDate.Location = new System.Drawing.Point(375, 87);
            dtpToDate.Size = new System.Drawing.Size(160, 27);
            dtpToDate.Value = System.DateTime.Today;

            // Nút xem báo cáo
            btnViewReport.Text = "Xem báo cáo";
            btnViewReport.Font = new System.Drawing.Font(
                "Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnViewReport.Location = new System.Drawing.Point(560, 83);
            btnViewReport.Size = new System.Drawing.Size(130, 36);
            btnViewReport.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            btnViewReport.ForeColor = System.Drawing.Color.White;
            btnViewReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewReport.FlatAppearance.BorderSize = 0;
            btnViewReport.Cursor = System.Windows.Forms.Cursors.Hand;

            // Nút làm mới
            btnRefresh.Text = "Làm mới";
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            btnRefresh.Location = new System.Drawing.Point(705, 83);
            btnRefresh.Size = new System.Drawing.Size(110, 36);
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;

            // Nút xuất Excel
            btnExport.Text = "Xuất Excel";
            btnExport.Font = new System.Drawing.Font(
                "Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnExport.Location = new System.Drawing.Point(830, 83);
            btnExport.Size = new System.Drawing.Size(130, 36);
            btnExport.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnExport.ForeColor = System.Drawing.Color.White;
            btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Cursor = System.Windows.Forms.Cursors.Hand;

            // Thẻ doanh thu
            pnlRevenue.Location = new System.Drawing.Point(25, 145);
            pnlRevenue.Size = new System.Drawing.Size(325, 110);
            pnlRevenue.BackColor = System.Drawing.Color.White;
            pnlRevenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblRevenueTitle.Text = "TỔNG DOANH THU";
            lblRevenueTitle.Font = new System.Drawing.Font(
                "Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblRevenueTitle.ForeColor = System.Drawing.Color.FromArgb(90, 100, 110);
            lblRevenueTitle.Location = new System.Drawing.Point(15, 15);
            lblRevenueTitle.Size = new System.Drawing.Size(290, 25);

            lblRevenue.Text = "0 đ";
            lblRevenue.Font = new System.Drawing.Font(
                "Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblRevenue.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
            lblRevenue.Location = new System.Drawing.Point(15, 48);
            lblRevenue.Size = new System.Drawing.Size(295, 45);
            lblRevenue.AutoEllipsis = true;

            pnlRevenue.Controls.Add(lblRevenueTitle);
            pnlRevenue.Controls.Add(lblRevenue);

            // Thẻ số hóa đơn
            pnlOrders.Location = new System.Drawing.Point(385, 145);
            pnlOrders.Size = new System.Drawing.Size(325, 110);
            pnlOrders.BackColor = System.Drawing.Color.White;
            pnlOrders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblOrdersTitle.Text = "SỐ LƯỢNG HÓA ĐƠN";
            lblOrdersTitle.Font = new System.Drawing.Font(
                "Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(90, 100, 110);
            lblOrdersTitle.Location = new System.Drawing.Point(15, 15);
            lblOrdersTitle.Size = new System.Drawing.Size(290, 25);

            lblOrders.Text = "0";
            lblOrders.Font = new System.Drawing.Font(
                "Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblOrders.ForeColor = System.Drawing.Color.FromArgb(0, 123, 255);
            lblOrders.Location = new System.Drawing.Point(15, 48);
            lblOrders.Size = new System.Drawing.Size(290, 45);

            pnlOrders.Controls.Add(lblOrdersTitle);
            pnlOrders.Controls.Add(lblOrders);

            // Thẻ sản phẩm đã bán
            pnlProducts.Location = new System.Drawing.Point(745, 145);
            pnlProducts.Size = new System.Drawing.Size(325, 110);
            pnlProducts.BackColor = System.Drawing.Color.White;
            pnlProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblProductsTitle.Text = "SỐ LƯỢNG SẢN PHẨM ĐÃ BÁN";
            lblProductsTitle.Font = new System.Drawing.Font(
                "Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblProductsTitle.ForeColor = System.Drawing.Color.FromArgb(90, 100, 110);
            lblProductsTitle.Location = new System.Drawing.Point(15, 15);
            lblProductsTitle.Size = new System.Drawing.Size(295, 25);

            lblProducts.Text = "0";
            lblProducts.Font = new System.Drawing.Font(
                "Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            lblProducts.ForeColor = System.Drawing.Color.FromArgb(255, 153, 0);
            lblProducts.Location = new System.Drawing.Point(15, 48);
            lblProducts.Size = new System.Drawing.Size(290, 45);

            pnlProducts.Controls.Add(lblProductsTitle);
            pnlProducts.Controls.Add(lblProducts);

            // Bảng báo cáo
            dgvReports.Location = new System.Drawing.Point(25, 280);
            dgvReports.Size = new System.Drawing.Size(1045, 365);
            dgvReports.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            dgvReports.BackgroundColor = System.Drawing.Color.White;
            dgvReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dgvReports.RowHeadersVisible = false;
            dgvReports.ReadOnly = true;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.MultiSelect = false;
            dgvReports.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvReports.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvReports.AutoGenerateColumns = true;
            dgvReports.ColumnHeadersHeight = 38;
            dgvReports.ColumnHeadersDefaultCellStyle.Font =
                new System.Drawing.Font(
                    "Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(35, 75, 130);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor =
                System.Drawing.Color.White;
            dgvReports.RowTemplate.Height = 32;
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(245, 248, 252);

            // Trạng thái
            lblStatus.Text = "Sẵn sàng";
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStatus.ForeColor = System.Drawing.Color.DimGray;
            lblStatus.Location = new System.Drawing.Point(25, 660);
            lblStatus.Size = new System.Drawing.Size(1045, 25);
            lblStatus.Anchor =
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left;

            // Đưa điều khiển vào Form
            Controls.Add(lblTitle);

            Controls.Add(lblFromDate);
            Controls.Add(dtpFromDate);
            Controls.Add(lblToDate);
            Controls.Add(dtpToDate);

            Controls.Add(btnViewReport);
            Controls.Add(btnRefresh);
            Controls.Add(btnExport);

            Controls.Add(pnlRevenue);
            Controls.Add(pnlOrders);
            Controls.Add(pnlProducts);

            Controls.Add(dgvReports);
            Controls.Add(lblStatus);

            pnlRevenue.ResumeLayout(false);
            pnlOrders.ResumeLayout(false);
            pnlProducts.ResumeLayout(false);

            ((System.ComponentModel.ISupportInitialize)dgvReports).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
