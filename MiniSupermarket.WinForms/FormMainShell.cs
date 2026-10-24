using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public class FormMainShell : Form
    {
        private Panel panelSidebar;
        private Panel panelTopHeader;
        private Panel panelMainContent;
        private Panel panelLogo;
        private Panel panelLogout;

        private Label lblLogo;
        private Label lblTitle;
        private Label lblUserInfo;

        private Button btnPOS;
        private Button btnCategory;
        private Button btnProduct;
        private Button btnCustomer;
        private Button btnLeaderboard;
        private Button btnReports;
        private Button btnUserManage;
        private Button btnSettings;
        private Button btnLogout;
        // Biến lưu Form con đang mở
        private Form _activeForm = null;

        private readonly Color sidebarColor =
            Color.FromArgb(0, 77, 64);

        private readonly Color contentColor =
            Color.FromArgb(245, 247, 246);

        private readonly Color activeColor =
            Color.FromArgb(0, 121, 107);

        private readonly Color accentColor =
            Color.FromArgb(239, 180, 58);

        private async Task<bool> CheckApiConnectionAsync()
        {
            try
            {
                using (HttpResponseMessage response =
                    await ApiClientService.Client.GetAsync("Categories"))
                {
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể kết nối API.\n" +
                    "Hãy kiểm tra MiniSupermarket.API đã chạy chưa.\n\n" +
                    ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        private void ApplyRolePermissions(string role)
        {
            // Ẩn toàn bộ menu trước, sau đó chỉ mở quyền cần thiết
            btnPOS.Visible = false;
            btnCategory.Visible = false;
            btnProduct.Visible = false;
            btnCustomer.Visible = false;
            btnLeaderboard.Visible = false;
            btnReports.Visible = false;
            btnUserManage.Visible = false;
            btnSettings.Visible = false;

            switch ((role ?? "").Trim().ToUpperInvariant())
            {
                case "ADMIN":
                    btnPOS.Visible = true;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnCustomer.Visible = true;
                    btnLeaderboard.Visible = true;
                    btnReports.Visible = true;
                    btnUserManage.Visible = true;
                    btnSettings.Visible = true;
                    break;

                case "CASHIER":
                    btnPOS.Visible = true;
                    btnCustomer.Visible = true;
                    btnLeaderboard.Visible = true;
                    btnSettings.Visible = true;
                    break;

                case "WAREHOUSE":
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnSettings.Visible = true;
                    break;

                default:
                    MessageBox.Show(
                        "Tài khoản chưa được cấp quyền hợp lệ!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.Close();
                    break;
            }
        }
        public FormMainShell()
        {
            InitializeComponent();
            this.Load += FormMainShell_Load;



        }

        private void InitializeComponent()
        {
            // Cấu hình Form
            this.Text =
                "Bách hóa Tổng hợp Hoàng Dung | Quản lý Kho và Thanh toán Điện tử";

            this.Size = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = contentColor;
            this.Font = new Font("Segoe UI", 10F);
            this.AutoScaleMode = AutoScaleMode.Font;

            // Khởi tạo các Panel
            panelSidebar = new Panel();
            panelTopHeader = new Panel();
            panelMainContent = new Panel();
            panelLogo = new Panel();
            panelLogout = new Panel();

            // =========================
            // SIDEBAR
            // =========================
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 250;
            panelSidebar.BackColor = sidebarColor;

            // Logo thương hiệu
            panelLogo.Height = 105;
            panelLogo.BackColor = sidebarColor;

            lblLogo = new Label();
            lblLogo.Text =
                "BÁCH HÓA\r\nTỔNG HỢP HOÀNG DUNG";
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font(
                "Segoe UI", 12F, FontStyle.Bold);
            lblLogo.Dock = DockStyle.Fill;
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;

            panelLogo.Controls.Add(lblLogo);

            // Panel đăng xuất dưới đáy
            panelLogout.Dock = DockStyle.Bottom;
            panelLogout.Height = 60;
            panelLogout.BackColor = sidebarColor;
            panelLogout.Padding = new Padding(10, 5, 10, 5);

            btnLogout = CreateMenuButton("⏻  Đăng xuất");
            btnLogout.Dock = DockStyle.Fill;
            btnLogout.Click += btnLogout_Click;

            panelLogout.Controls.Add(btnLogout);

            // Các nút điều hướng
            btnPOS = CreateMenuButton("▣  Thanh toán điện tử (POS)");
            btnCategory = CreateMenuButton("▤  Nhóm hàng trong kho");
            btnProduct = CreateMenuButton("▦  Quản lý tồn kho");
            btnCustomer = CreateMenuButton("♙  Khách hàng");
            btnLeaderboard = CreateMenuButton("🏆  BXH Điểm tích lũy");
            btnReports = CreateMenuButton("▥  Báo cáo kho và doanh thu");
            btnUserManage = CreateMenuButton("👤  Quản trị tài khoản");
            btnSettings = CreateMenuButton("⚙  Cài đặt hệ thống");

            btnPOS.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormPOS(),
                    "THANH TOÁN ĐIỆN TỬ (POS)",
                    btnPOS);
            };

            btnCategory.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormCategoryManagement(),
                    "NHÓM HÀNG TRONG KHO",
                    btnCategory);
            };

            btnProduct.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormProduct(),
                    "QUẢN LÝ TỒN KHO",
                    btnProduct);
            };

            btnCustomer.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormCustomerManagement(),
                    "QUẢN LÝ KHÁCH HÀNG",
                    btnCustomer);
            };

            btnLeaderboard.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormCustomerPortal(),
                    "BẢNG XẾP HẠNG ĐIỂM TÍCH LŨY",
                    btnLeaderboard);
            };

            btnReports.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormReports(),
                    "BÁO CÁO KHO & DOANH THU",
                    btnReports);
            };

            btnUserManage.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormUserManage(),
                    "QUẢN TRỊ TÀI KHOẢN",
                    btnUserManage);
            };

            btnSettings.Click += (s, e) =>
            {
                OpenChildForm(
                    new FormSettings(),
                    "CÀI ĐẶT HỆ THỐNG",
                    btnSettings);
            };

            // Dùng FlowLayoutPanel để xếp nút theo chiều dọc
            FlowLayoutPanel menuPanel = new FlowLayoutPanel();
            menuPanel.Dock = DockStyle.Fill;
            menuPanel.FlowDirection = FlowDirection.TopDown;
            menuPanel.WrapContents = false;
            menuPanel.AutoScroll = true;
            menuPanel.BackColor = sidebarColor;
            menuPanel.Padding = new Padding(10, 10, 10, 0);

            menuPanel.Controls.Add(btnPOS);
            menuPanel.Controls.Add(btnCategory);
            menuPanel.Controls.Add(btnProduct);
            menuPanel.Controls.Add(btnCustomer);
            menuPanel.Controls.Add(btnLeaderboard);
            menuPanel.Controls.Add(btnReports);
            menuPanel.Controls.Add(btnUserManage);
            menuPanel.Controls.Add(btnSettings);

            panelSidebar.Controls.Add(menuPanel);
            panelSidebar.Controls.Add(panelLogout);
            panelSidebar.Controls.Add(panelLogo);

            // =========================
            // TOP HEADER
            // =========================
            panelTopHeader.Dock = DockStyle.Top;
            panelTopHeader.Height = 72;
            panelTopHeader.BackColor = Color.White;
            panelTopHeader.Padding = new Padding(24, 0, 24, 0);

            lblTitle = new Label();
            lblTitle.Text = "TỔNG QUAN HỆ THỐNG";
            lblTitle.Font = new Font(
                "Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = sidebarColor;
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblUserInfo = new Label();
            lblUserInfo.Text = "Xin chào: Quản trị viên";
            lblUserInfo.Font = new Font("Segoe UI", 10F);
            lblUserInfo.ForeColor = Color.FromArgb(80, 90, 105);
            lblUserInfo.AutoSize = true;
            lblUserInfo.Dock = DockStyle.Right;
            lblUserInfo.TextAlign = ContentAlignment.MiddleRight;

            panelTopHeader.Controls.Add(lblUserInfo);
            panelTopHeader.Controls.Add(lblTitle);

            // =========================
            // MAIN CONTENT
            // =========================
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.BackColor = contentColor;
            panelMainContent.Padding = new Padding(20);

            // Nội dung mặc định
            Label lblWelcome = new Label();
            lblWelcome.Text =
                "BÁCH HÓA TỔNG HỢP HOÀNG DUNG\r\n\r\n" +
                "Giải pháp Quản lý Kho và Thanh toán Điện tử\r\n" +
                "Cửa hàng Mini";
            lblWelcome.Dock = DockStyle.Fill;
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Font = new Font(
                "Segoe UI", 18F, FontStyle.Bold);
            lblWelcome.ForeColor = sidebarColor;

            // Thanh nhấn thương hiệu cho vùng nội dung
            Panel welcomeAccent = new Panel();
            welcomeAccent.Height = 5;
            welcomeAccent.Dock = DockStyle.Top;
            welcomeAccent.BackColor = accentColor;
            panelMainContent.Controls.Add(welcomeAccent);
            panelMainContent.Controls.Add(lblWelcome);

            // =========================
            // THÊM CONTROLS VÀO FORM
            // =========================
            this.Controls.Add(panelMainContent);
            this.Controls.Add(panelTopHeader);
            this.Controls.Add(panelSidebar);

            // Chọn mặc định chức năng POS
            SetActiveButton(btnPOS);
        }

        // Hàm tạo nút điều hướng
        private Button CreateMenuButton(string text)
        {
            Button btn = new Button();

            btn.Text = text;
            btn.Width = 228;
            btn.Height = 48;
            btn.Margin = new Padding(0, 0, 0, 6);

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.BackColor = sidebarColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(8, 0, 0, 0);
            btn.UseVisualStyleBackColor = false;

            btn.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(0, 105, 92);

            return btn;
        }

        // Đánh dấu nút đang được chọn
        private void SetActiveButton(Button activeButton)
        {
            Button[] buttons =
            {
                btnPOS,
                btnCategory,
                btnProduct,
                btnCustomer,
                btnLeaderboard,
                btnReports,
                btnUserManage,
                btnSettings
            };

            foreach (Button btn in buttons)
            {
                if (btn != null)
                {
                    btn.BackColor = sidebarColor;
                }
            }

            if (activeButton != null)
            {
                activeButton.BackColor = activeColor;
            }
        }

        // Xử lý nút đăng xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất phiên làm việc?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                SessionManager.Clear();

                this.Hide();

                using (FormLogin loginForm = new FormLogin())
                {
                    loginForm.ShowDialog();
                }

                this.Close();
            }
        }
        private void OpenChildForm(
            Form childForm,
            string screenTitle,
            Button senderButton)
        {
            // Đóng Form con hiện tại
            if (_activeForm != null)
            {
                _activeForm.Close();
                _activeForm.Dispose();
                _activeForm = null;
            }

            // Cập nhật nút menu đang chọn
            HighlightActiveButton(senderButton);

            // Cấu hình Form con để nhúng vào Panel
            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Xóa nội dung cũ và thêm Form mới
            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(childForm);
            panelMainContent.Tag = childForm;

            // Cập nhật tiêu đề
            lblTitle.Text = screenTitle;

            childForm.BringToFront();
            childForm.Show();
        }

        private void HighlightActiveButton(Button activeButton)
        {
            Button[] menuButtons =
            {
        btnPOS,
        btnCategory,
        btnProduct,
        btnCustomer,
        btnLeaderboard,
        btnReports,
        btnUserManage,
        btnSettings
    };

            foreach (Button btn in menuButtons)
            {
                btn.BackColor = sidebarColor;
            }

            activeButton.BackColor = activeColor;
        }
        private async void FormMainShell_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text =
                $"Nhân viên: {SessionManager.CurrentUsername} | " +
                $"Vai trò: [{SessionManager.CurrentRole}]";

            ApplyRolePermissions(SessionManager.CurrentRole);

            if (IsDisposed)
                return;

            bool connected = await CheckApiConnectionAsync();

            if (!connected || IsDisposed)
                return;

            if (!string.IsNullOrWhiteSpace(SessionManager.CurrentRole))
            {
                OpenDefaultScreenByRole(SessionManager.CurrentRole);
            }
        }
        private void OpenDefaultScreenByRole(string role)
        {
            switch ((role ?? "").Trim().ToUpperInvariant())
            {
                case "ADMIN":
                case "WAREHOUSE":
                    OpenChildForm(
                        new FormCategoryManagement(),
                        "QUẢN LÝ NHÓM HÀNG TRONG KHO",
                        btnCategory);
                    break;

                case "CASHIER":
                    OpenChildForm(
                        new FormCustomerManagement(),
                        "QUẢN LÝ KHÁCH HÀNG & TÍCH ĐIỂM",
                        btnCustomer);
                    break;
            }
        }
        private void btnCategory_Click(object sender, EventArgs e)
        {
            OpenChildForm(
                new FormCategoryManagement(),
                "QUẢN LÝ DANH MỤC SẢN PHẨM",
                btnCategory);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(
                new FormCustomerManagement(),
                "QUẢN LÝ KHÁCH HÀNG THÂN THIẾT",
                btnCustomer);
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
           
        }

    }
}