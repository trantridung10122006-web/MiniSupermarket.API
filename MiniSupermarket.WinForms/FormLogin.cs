using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        private SaveFileDialog saveFileDialog1;
        private GroupBox groupBox1;
        private TextBox txtPassword;
        private TextBox txtUser;
        private Button btnLogin;
        private Label label2;
        private Label label1;

        // Khởi tạo HttpClient kết nối đến API Backend
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7054/api/")
        };

        public FormLogin()
        {
            InitializeComponent();
        }

        // Sự kiện khi người dùng bấm nút Đăng nhập
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Ràng buộc cơ bản phía Client
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Vô hiệu hóa nút đăng nhập trong khi chờ xử lý API để tránh click nhiều lần
            btnLogin.Enabled = false;

            try
            {
                var loginData = new { Username = username, Password = password };
                var response = await _client.PostAsJsonAsync("auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // Cấu hình Case-Insensitive để đọc cả key "Token"/"token" hay "Role"/"role"
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var doc = JsonDocument.Parse(jsonString);

                    // Trích xuất thông tin an toàn với TryGetProperty
                    if (doc.RootElement.TryGetProperty("token", out var tokenProp))
                    {
                        SessionManager.JwtToken = tokenProp.GetString() ?? string.Empty;
                    }

                    if (doc.RootElement.TryGetProperty("role", out var roleProp))
                    {
                        SessionManager.CurrentRole = roleProp.GetString() ?? string.Empty;
                    }

                    MessageBox.Show($"Đăng nhập thành công với quyền: {SessionManager.CurrentRole}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở Form quản lý chính và ẩn Form đăng nhập đi
                    FormCategoryManagement mainForm = new FormCategoryManagement();
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close(); // Đóng hẳn ứng dụng khi form chính tắt
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến Server API: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Mở lại nút đăng nhập
                btnLogin.Enabled = true;
            }
        }

        private void InitializeComponent()
        {
            this.saveFileDialog1 = new SaveFileDialog();
            this.groupBox1 = new GroupBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.txtPassword = new TextBox();
            this.txtUser = new TextBox();
            this.btnLogin = new Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Location = new System.Drawing.Point(124, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(631, 331);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ĐĂNG NHẬP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mật Khẩu";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tài Khoản";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(212, 155);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(207, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true; // Ẩn mật khẩu dạng dấu chấm/sao
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(212, 82);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(207, 20);
            this.txtUser.TabIndex = 1;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(265, 220);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(120, 30); // Điều chỉnh chiều rộng để chữ không bị che
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Đăng nhập hệ thống";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click); // Đăng ký sự kiện Click
            // 
            // FormLogin
            // 
            this.ClientSize = new System.Drawing.Size(867, 442);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormLogin";
            this.StartPosition = FormStartPosition.CenterScreen; // Hiển thị Form ra giữa màn hình
            this.Text = "Đăng Nhập";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}