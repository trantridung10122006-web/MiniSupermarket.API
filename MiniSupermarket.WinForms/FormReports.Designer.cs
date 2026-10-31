namespace MiniSupermarket.WinForms
{
    partial class FormReports
    {
        private System.ComponentModel.IContainer components = null;

        // Bố cục chính
        private System.Windows.Forms.TableLayoutPanel tlpMain;

        // 1. Header & Bộ lọc
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlTitleBox;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.FlowLayoutPanel flpFilter;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnViewReport;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;

        // 2. Thẻ KPI
        private System.Windows.Forms.TableLayoutPanel tlpCards;

        private System.Windows.Forms.Panel pnlRevenue;
        private System.Windows.Forms.Panel pnlRevenueStrip;
        private System.Windows.Forms.Label lblRevenueTitle;
        private System.Windows.Forms.Label lblRevenue;
        private System.Windows.Forms.Label lblRevenueSub;

        private System.Windows.Forms.Panel pnlOrders;
        private System.Windows.Forms.Panel pnlOrdersStrip;
        private System.Windows.Forms.Label lblOrdersTitle;
        private System.Windows.Forms.Label lblOrders;
        private System.Windows.Forms.Label lblOrdersSub;

        private System.Windows.Forms.Panel pnlProducts;
        private System.Windows.Forms.Panel pnlProductsStrip;
        private System.Windows.Forms.Label lblProductsTitle;
        private System.Windows.Forms.Label lblProducts;
        private System.Windows.Forms.Label lblProductsSub;

        private System.Windows.Forms.Panel pnlLowStock;
        private System.Windows.Forms.Panel pnlLowStockStrip;
        private System.Windows.Forms.Label lblLowStockTitle;
        private System.Windows.Forms.Label lblLowStock;
        private System.Windows.Forms.Label lblLowStockSub;

        // 3. Biểu đồ & Top bán chạy
        private System.Windows.Forms.TableLayoutPanel tlpAnalytics;
        private System.Windows.Forms.Panel pnlTrendChart;
        private System.Windows.Forms.Panel pnlCategoryChart;
        private System.Windows.Forms.Panel pnlBestSellers;
        private System.Windows.Forms.Label lblBestSellersTitle;
        private System.Windows.Forms.ListBox lstBestSellers;

        // 4. Bảng chi tiết đơn hàng
        private System.Windows.Forms.Panel pnlGridCard;
        private System.Windows.Forms.Panel pnlGridHeader;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.Label lblGridSummary;
        private System.Windows.Forms.DataGridView dgvReports;

        // 5. Thanh trạng thái
        private System.Windows.Forms.Label lblStatus;

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
            // Bố cục chính
            tlpMain = new System.Windows.Forms.TableLayoutPanel();

            // 1. Header & Filters
            pnlHeader = new System.Windows.Forms.Panel();
            pnlTitleBox = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            flpFilter = new System.Windows.Forms.FlowLayoutPanel();
            lblFromDate = new System.Windows.Forms.Label();
            dtpFromDate = new System.Windows.Forms.DateTimePicker();
            lblToDate = new System.Windows.Forms.Label();
            dtpToDate = new System.Windows.Forms.DateTimePicker();
            btnViewReport = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();

            // 2. Thẻ KPI
            tlpCards = new System.Windows.Forms.TableLayoutPanel();

            pnlRevenue = new System.Windows.Forms.Panel();
            pnlRevenueStrip = new System.Windows.Forms.Panel();
            lblRevenueTitle = new System.Windows.Forms.Label();
            lblRevenue = new System.Windows.Forms.Label();
            lblRevenueSub = new System.Windows.Forms.Label();

            pnlOrders = new System.Windows.Forms.Panel();
            pnlOrdersStrip = new System.Windows.Forms.Panel();
            lblOrdersTitle = new System.Windows.Forms.Label();
            lblOrders = new System.Windows.Forms.Label();
            lblOrdersSub = new System.Windows.Forms.Label();

            pnlProducts = new System.Windows.Forms.Panel();
            pnlProductsStrip = new System.Windows.Forms.Panel();
            lblProductsTitle = new System.Windows.Forms.Label();
            lblProducts = new System.Windows.Forms.Label();
            lblProductsSub = new System.Windows.Forms.Label();

            pnlLowStock = new System.Windows.Forms.Panel();
            pnlLowStockStrip = new System.Windows.Forms.Panel();
            lblLowStockTitle = new System.Windows.Forms.Label();
            lblLowStock = new System.Windows.Forms.Label();
            lblLowStockSub = new System.Windows.Forms.Label();

            // 3. Biểu đồ & Top bán chạy
            tlpAnalytics = new System.Windows.Forms.TableLayoutPanel();
            pnlTrendChart = new DoubleBufferedPanel();
            pnlCategoryChart = new DoubleBufferedPanel();
            pnlBestSellers = new System.Windows.Forms.Panel();
            lblBestSellersTitle = new System.Windows.Forms.Label();
            lstBestSellers = new System.Windows.Forms.ListBox();

            // 4. Bảng chi tiết hóa đơn
            pnlGridCard = new System.Windows.Forms.Panel();
            pnlGridHeader = new System.Windows.Forms.Panel();
            lblGridTitle = new System.Windows.Forms.Label();
            lblGridSummary = new System.Windows.Forms.Label();
            dgvReports = new System.Windows.Forms.DataGridView();

            // 5. Trạng thái
            lblStatus = new System.Windows.Forms.Label();

            tlpMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlTitleBox.SuspendLayout();
            flpFilter.SuspendLayout();
            tlpCards.SuspendLayout();
            pnlRevenue.SuspendLayout();
            pnlOrders.SuspendLayout();
            pnlProducts.SuspendLayout();
            pnlLowStock.SuspendLayout();
            tlpAnalytics.SuspendLayout();
            pnlBestSellers.SuspendLayout();
            pnlGridCard.SuspendLayout();
            pnlGridHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReports).BeginInit();
            SuspendLayout();

            // =====================================================
            // Cấu hình Form
            // =====================================================
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1200, 750);
            MinimumSize = new System.Drawing.Size(1000, 600);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "BÁO CÁO - THỐNG KÊ DOANH THU";
            BackColor = System.Drawing.Color.FromArgb(245, 247, 246);

            // =====================================================
            // Bố cục chính: tlpMain (Master TableLayoutPanel)
            // =====================================================
            tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMain.RowCount = 5;
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));  // Header & Bộ lọc
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));  // 4 Thẻ KPI
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 224F)); // 3 Khối biểu đồ & Top SP
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F)); // Bảng chi tiết hóa đơn
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));  // Thanh trạng thái
            tlpMain.Padding = new System.Windows.Forms.Padding(16, 8, 16, 6);
            tlpMain.Margin = new System.Windows.Forms.Padding(0);
            tlpMain.BackColor = System.Drawing.Color.Transparent;

            // =====================================================
            // 1. Header & Bộ lọc (Row 0)
            // =====================================================
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            pnlHeader.BackColor = System.Drawing.Color.Transparent;

            // Tiêu đề & phụ đề
            pnlTitleBox.Dock = System.Windows.Forms.DockStyle.Left;
            pnlTitleBox.AutoSize = true;
            pnlTitleBox.BackColor = System.Drawing.Color.Transparent;

            lblTitle.Text = "BÁO CÁO & THỐNG KÊ DOANH THU";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 77, 64);
            lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitle.Height = 26;
            lblTitle.AutoSize = true;

            lblSubtitle.Text = "Dữ liệu kinh doanh, phân tích tỷ trọng và xu hướng bán hàng";
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblSubtitle.Height = 18;
            lblSubtitle.AutoSize = true;

            pnlTitleBox.Controls.Add(lblSubtitle);
            pnlTitleBox.Controls.Add(lblTitle);

            // Bộ lọc & tác vụ (flpFilter)
            flpFilter.Dock = System.Windows.Forms.DockStyle.Right;
            flpFilter.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            flpFilter.WrapContents = false;
            flpFilter.AutoSize = true;
            flpFilter.BackColor = System.Drawing.Color.Transparent;
            flpFilter.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);

            lblFromDate.Text = "Từ:";
            lblFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblFromDate.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            lblFromDate.AutoSize = true;
            lblFromDate.Margin = new System.Windows.Forms.Padding(4, 7, 2, 0);

            dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFromDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpFromDate.Width = 110;
            dtpFromDate.Margin = new System.Windows.Forms.Padding(2, 4, 6, 0);
            dtpFromDate.Value = System.DateTime.Today.AddDays(-30);

            lblToDate.Text = "Đến:";
            lblToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblToDate.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            lblToDate.AutoSize = true;
            lblToDate.Margin = new System.Windows.Forms.Padding(4, 7, 2, 0);

            dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpToDate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpToDate.Width = 110;
            dtpToDate.Margin = new System.Windows.Forms.Padding(2, 4, 10, 0);
            dtpToDate.Value = System.DateTime.Today;

            btnViewReport.Text = "🔍 Xem báo cáo";
            btnViewReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnViewReport.Size = new System.Drawing.Size(120, 32);
            btnViewReport.BackColor = System.Drawing.Color.FromArgb(0, 121, 107);
            btnViewReport.ForeColor = System.Drawing.Color.White;
            btnViewReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewReport.FlatAppearance.BorderSize = 0;
            btnViewReport.Cursor = System.Windows.Forms.Cursors.Hand;
            btnViewReport.Margin = new System.Windows.Forms.Padding(2, 2, 4, 0);

            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnRefresh.Size = new System.Drawing.Size(100, 32);
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(96, 125, 139);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 4, 0);

            btnExport.Text = "📊 Xuất Excel";
            btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnExport.Size = new System.Drawing.Size(115, 32);
            btnExport.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            btnExport.ForeColor = System.Drawing.Color.White;
            btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExport.Margin = new System.Windows.Forms.Padding(2, 2, 0, 0);

            flpFilter.Controls.Add(lblFromDate);
            flpFilter.Controls.Add(dtpFromDate);
            flpFilter.Controls.Add(lblToDate);
            flpFilter.Controls.Add(dtpToDate);
            flpFilter.Controls.Add(btnViewReport);
            flpFilter.Controls.Add(btnRefresh);
            flpFilter.Controls.Add(btnExport);

            pnlHeader.Controls.Add(pnlTitleBox);
            pnlHeader.Controls.Add(flpFilter);

            // =====================================================
            // 2. Thẻ KPI (Row 1)
            // =====================================================
            tlpCards.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpCards.ColumnCount = 4;
            tlpCards.RowCount = 1;
            tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tlpCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpCards.BackColor = System.Drawing.Color.Transparent;
            tlpCards.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);

            // Thẻ 1: Tổng doanh thu
            pnlRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRevenue.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            pnlRevenue.BackColor = System.Drawing.Color.White;
            pnlRevenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            pnlRevenueStrip.Dock = System.Windows.Forms.DockStyle.Top;
            pnlRevenueStrip.Height = 3;
            pnlRevenueStrip.BackColor = System.Drawing.Color.FromArgb(0, 137, 123);

            lblRevenueTitle.Text = "💳  TỔNG DOANH THU";
            lblRevenueTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblRevenueTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblRevenueTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblRevenueTitle.Height = 22;
            lblRevenueTitle.Padding = new System.Windows.Forms.Padding(10, 4, 0, 0);

            lblRevenue.Text = "0 đ";
            lblRevenue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblRevenue.ForeColor = System.Drawing.Color.FromArgb(0, 121, 107);
            lblRevenue.Dock = System.Windows.Forms.DockStyle.Top;
            lblRevenue.Height = 32;
            lblRevenue.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            lblRevenue.AutoEllipsis = true;

            lblRevenueSub.Text = "Doanh số trong kỳ lọc";
            lblRevenueSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblRevenueSub.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblRevenueSub.Dock = System.Windows.Forms.DockStyle.Top;
            lblRevenueSub.Height = 18;
            lblRevenueSub.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            pnlRevenue.Controls.Add(lblRevenueSub);
            pnlRevenue.Controls.Add(lblRevenue);
            pnlRevenue.Controls.Add(lblRevenueTitle);
            pnlRevenue.Controls.Add(pnlRevenueStrip);

            // Thẻ 2: Số lượng hóa đơn
            pnlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlOrders.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            pnlOrders.BackColor = System.Drawing.Color.White;
            pnlOrders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            pnlOrdersStrip.Dock = System.Windows.Forms.DockStyle.Top;
            pnlOrdersStrip.Height = 3;
            pnlOrdersStrip.BackColor = System.Drawing.Color.FromArgb(30, 136, 229);

            lblOrdersTitle.Text = "🧾  SỐ LƯỢNG HÓA ĐƠN";
            lblOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblOrdersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblOrdersTitle.Height = 22;
            lblOrdersTitle.Padding = new System.Windows.Forms.Padding(10, 4, 0, 0);

            lblOrders.Text = "0";
            lblOrders.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblOrders.ForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            lblOrders.Dock = System.Windows.Forms.DockStyle.Top;
            lblOrders.Height = 32;
            lblOrders.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            lblOrdersSub.Text = "Hóa đơn đã hoàn thành";
            lblOrdersSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblOrdersSub.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblOrdersSub.Dock = System.Windows.Forms.DockStyle.Top;
            lblOrdersSub.Height = 18;
            lblOrdersSub.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            pnlOrders.Controls.Add(lblOrdersSub);
            pnlOrders.Controls.Add(lblOrders);
            pnlOrders.Controls.Add(lblOrdersTitle);
            pnlOrders.Controls.Add(pnlOrdersStrip);

            // Thẻ 3: Khách mới hôm nay
            pnlProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlProducts.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            pnlProducts.BackColor = System.Drawing.Color.White;
            pnlProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            pnlProductsStrip.Dock = System.Windows.Forms.DockStyle.Top;
            pnlProductsStrip.Height = 3;
            pnlProductsStrip.BackColor = System.Drawing.Color.FromArgb(251, 140, 0);

            lblProductsTitle.Text = "👥  KHÁCH MỚI HÔM NAY";
            lblProductsTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblProductsTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblProductsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblProductsTitle.Height = 22;
            lblProductsTitle.Padding = new System.Windows.Forms.Padding(10, 4, 0, 0);

            lblProducts.Text = "0";
            lblProducts.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblProducts.ForeColor = System.Drawing.Color.FromArgb(230, 81, 0);
            lblProducts.Dock = System.Windows.Forms.DockStyle.Top;
            lblProducts.Height = 32;
            lblProducts.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            lblProductsSub.Text = "Khách hàng đăng ký mới";
            lblProductsSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblProductsSub.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblProductsSub.Dock = System.Windows.Forms.DockStyle.Top;
            lblProductsSub.Height = 18;
            lblProductsSub.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            pnlProducts.Controls.Add(lblProductsSub);
            pnlProducts.Controls.Add(lblProducts);
            pnlProducts.Controls.Add(lblProductsTitle);
            pnlProducts.Controls.Add(pnlProductsStrip);

            // Thẻ 4: Mặt hàng sắp hết
            pnlLowStock.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLowStock.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            pnlLowStock.BackColor = System.Drawing.Color.White;
            pnlLowStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            pnlLowStockStrip.Dock = System.Windows.Forms.DockStyle.Top;
            pnlLowStockStrip.Height = 3;
            pnlLowStockStrip.BackColor = System.Drawing.Color.FromArgb(229, 57, 53);

            lblLowStockTitle.Text = "⚠️  MẶT HÀNG SẮP HẾT";
            lblLowStockTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblLowStockTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblLowStockTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblLowStockTitle.Height = 22;
            lblLowStockTitle.Padding = new System.Windows.Forms.Padding(10, 4, 0, 0);

            lblLowStock.Text = "0";
            lblLowStock.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblLowStock.ForeColor = System.Drawing.Color.FromArgb(198, 40, 40);
            lblLowStock.Dock = System.Windows.Forms.DockStyle.Top;
            lblLowStock.Height = 32;
            lblLowStock.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            lblLowStockSub.Text = "Cần nhập thêm tồn kho";
            lblLowStockSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblLowStockSub.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            lblLowStockSub.Dock = System.Windows.Forms.DockStyle.Top;
            lblLowStockSub.Height = 18;
            lblLowStockSub.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            pnlLowStock.Controls.Add(lblLowStockSub);
            pnlLowStock.Controls.Add(lblLowStock);
            pnlLowStock.Controls.Add(lblLowStockTitle);
            pnlLowStock.Controls.Add(pnlLowStockStrip);

            tlpCards.Controls.Add(pnlRevenue, 0, 0);
            tlpCards.Controls.Add(pnlOrders, 1, 0);
            tlpCards.Controls.Add(pnlProducts, 2, 0);
            tlpCards.Controls.Add(pnlLowStock, 3, 0);

            // =====================================================
            // 3. Biểu đồ & Top bán chạy: tlpAnalytics (Row 2)
            // =====================================================
            tlpAnalytics.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpAnalytics.ColumnCount = 3;
            tlpAnalytics.RowCount = 1;
            tlpAnalytics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            tlpAnalytics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            tlpAnalytics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            tlpAnalytics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpAnalytics.BackColor = System.Drawing.Color.Transparent;
            tlpAnalytics.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);

            // Biểu đồ Doanh thu theo ngày
            pnlTrendChart.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTrendChart.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            pnlTrendChart.BackColor = System.Drawing.Color.White;
            pnlTrendChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Biểu đồ Tỉ trọng ngành hàng
            pnlCategoryChart.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCategoryChart.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            pnlCategoryChart.BackColor = System.Drawing.Color.White;
            pnlCategoryChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Top 5 sản phẩm bán chạy
            pnlBestSellers.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBestSellers.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            pnlBestSellers.BackColor = System.Drawing.Color.White;
            pnlBestSellers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            lblBestSellersTitle.Text = "🏆  TOP 5 BÁN CHẠY";
            lblBestSellersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblBestSellersTitle.ForeColor = System.Drawing.Color.FromArgb(0, 77, 64);
            lblBestSellersTitle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            lblBestSellersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblBestSellersTitle.Height = 28;
            lblBestSellersTitle.Padding = new System.Windows.Forms.Padding(10, 6, 0, 0);

            lstBestSellers.Dock = System.Windows.Forms.DockStyle.Fill;
            lstBestSellers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lstBestSellers.Font = new System.Drawing.Font("Segoe UI", 9F);
            lstBestSellers.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            lstBestSellers.BackColor = System.Drawing.Color.White;
            lstBestSellers.IntegralHeight = false;
            lstBestSellers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            lstBestSellers.ItemHeight = 35;

            pnlBestSellers.Controls.Add(lstBestSellers);
            pnlBestSellers.Controls.Add(lblBestSellersTitle);

            tlpAnalytics.Controls.Add(pnlTrendChart, 0, 0);
            tlpAnalytics.Controls.Add(pnlCategoryChart, 1, 0);
            tlpAnalytics.Controls.Add(pnlBestSellers, 2, 0);

            // =====================================================
            // 4. Bảng chi tiết hóa đơn: pnlGridCard (Row 3)
            // =====================================================
            pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGridCard.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            pnlGridCard.BackColor = System.Drawing.Color.White;
            pnlGridCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            pnlGridHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlGridHeader.Height = 30;
            pnlGridHeader.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            pnlGridHeader.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);

            lblGridTitle.Text = "📋  DANH SÁCH ĐƠN HÀNG CHI TIẾT";
            lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblGridTitle.ForeColor = System.Drawing.Color.FromArgb(0, 77, 64);
            lblGridTitle.Dock = System.Windows.Forms.DockStyle.Left;
            lblGridTitle.AutoSize = true;
            lblGridTitle.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            lblGridSummary.Text = "";
            lblGridSummary.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblGridSummary.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblGridSummary.Dock = System.Windows.Forms.DockStyle.Right;
            lblGridSummary.AutoSize = true;
            lblGridSummary.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            pnlGridHeader.Controls.Add(lblGridSummary);
            pnlGridHeader.Controls.Add(lblGridTitle);

            dgvReports.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvReports.BackgroundColor = System.Drawing.Color.White;
            dgvReports.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvReports.RowHeadersVisible = false;
            dgvReports.ReadOnly = true;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AllowUserToDeleteRows = false;
            dgvReports.MultiSelect = false;
            dgvReports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvReports.AutoGenerateColumns = false;
            dgvReports.ColumnHeadersHeight = 34;
            dgvReports.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 77, 64);
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvReports.RowTemplate.Height = 30;
            dgvReports.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            dgvReports.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(178, 223, 219);
            dgvReports.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvReports.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);

            pnlGridCard.Controls.Add(dgvReports);
            pnlGridCard.Controls.Add(pnlGridHeader);

            // =====================================================
            // 5. Thanh trạng thái: lblStatus (Row 4)
            // =====================================================
            lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            lblStatus.Text = "Sẵn sàng";
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblStatus.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);

            // Thêm các hàng vào tlpMain
            tlpMain.Controls.Add(pnlHeader, 0, 0);
            tlpMain.Controls.Add(tlpCards, 0, 1);
            tlpMain.Controls.Add(tlpAnalytics, 0, 2);
            tlpMain.Controls.Add(pnlGridCard, 0, 3);
            tlpMain.Controls.Add(lblStatus, 0, 4);

            Controls.Add(tlpMain);

            tlpMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlTitleBox.ResumeLayout(false);
            pnlTitleBox.PerformLayout();
            flpFilter.ResumeLayout(false);
            flpFilter.PerformLayout();
            tlpCards.ResumeLayout(false);
            pnlRevenue.ResumeLayout(false);
            pnlOrders.ResumeLayout(false);
            pnlProducts.ResumeLayout(false);
            pnlLowStock.ResumeLayout(false);
            tlpAnalytics.ResumeLayout(false);
            pnlBestSellers.ResumeLayout(false);
            pnlGridCard.ResumeLayout(false);
            pnlGridHeader.ResumeLayout(false);
            pnlGridHeader.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)dgvReports).EndInit();

            ResumeLayout(false);
        }
    }
}
