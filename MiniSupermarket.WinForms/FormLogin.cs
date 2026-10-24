using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            WinFormsTheme.Apply(this);
            ApplyLoginTheme();
        }

        private void ApplyLoginTheme()
        {
            Text = "Đăng nhập - Bách hóa Tổng hợp Hoàng Dung";
            ClientSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(24, 30, 48);

            groupBox1.Text = "ĐĂNG NHẬP HỆ THỐNG";
            groupBox1.Location = new Point(240, 120);
            groupBox1.Size = new Size(420, 300);
            groupBox1.BackColor = Color.White;
            groupBox1.ForeColor = Color.FromArgb(24, 30, 48);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            label1.Text = "Tài khoản";
            label2.Text = "Mật khẩu";
            label1.Location = new Point(42, 76);
            label2.Location = new Point(42, 130);
            label1.ForeColor = Color.FromArgb(70, 78, 94);
            label2.ForeColor = Color.FromArgb(70, 78, 94);
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            txtUser.Location = new Point(145, 72);
            txtUser.Size = new Size(230, 28);
            txtPassword.Location = new Point(145, 126);
            txtPassword.Size = new Size(230, 28);

            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.Location = new Point(42, 205);
            btnLogin.Size = new Size(333, 42);
            btnLogin.BackColor = Color.FromArgb(0, 121, 107);
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 105, 92);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Enabled = false;

                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                var response = await ApiClientService.Client.PostAsJsonAsync(
                    "auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString =
                        await response.Content.ReadAsStringAsync();

                    var doc = JsonDocument.Parse(jsonString);

                    if (!doc.RootElement.TryGetProperty(
                            "token", out var tokenElement) ||
                        !doc.RootElement.TryGetProperty(
                            "role", out var roleElement))
                    {
                        MessageBox.Show(
                            "Dữ liệu trả về từ API không đúng định dạng.",
                            "Lỗi dữ liệu",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    string token =
                        tokenElement.GetString() ?? string.Empty;

                    string role =
                        roleElement.GetString() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        MessageBox.Show(
                            "API chưa trả về JWT Token hợp lệ.",
                            "Lỗi đăng nhập",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Lưu thông tin phiên đăng nhập
                    SessionManager.JwtToken = token;
                    SessionManager.CurrentUsername = username;
                    SessionManager.CurrentRole = role;
                    SessionManager.LoginTime = DateTime.Now;

                    if (doc.RootElement.TryGetProperty("fullName", out var fnElement))
                    {
                        SessionManager.CurrentFullName = fnElement.GetString() ?? string.Empty;
                    }
                    if (doc.RootElement.TryGetProperty("customerId", out var cidElement) && cidElement.ValueKind == JsonValueKind.Number)
                    {
                        SessionManager.CurrentCustomerId = cidElement.GetInt32();
                    }
                    if (doc.RootElement.TryGetProperty("customerPhone", out var cpElement))
                    {
                        SessionManager.CurrentPhone = cpElement.GetString() ?? string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(SessionManager.CurrentFullName))
                    {
                        SessionManager.CurrentFullName = (username.ToLower() == "admin") ? "Chủ Web" : username;
                    }

                    MessageBox.Show(
                        $"Đăng nhập thành công!\n" +
                        $"Tài khoản: {username}\n" +
                        $"Vai trò: {role}",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Mở màn hình tương ứng vai trò
                    this.Hide();

                    if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase) ||
                        role.Equals("Khách Hàng", StringComparison.OrdinalIgnoreCase))
                    {
                        using (FormCustomerPortal customerForm = new FormCustomerPortal())
                        {
                            customerForm.ShowDialog();
                        }
                    }
                    else
                    {
                        using (FormMainShell mainForm = new FormMainShell())
                        {
                            mainForm.ShowDialog();
                        }
                    }

                    // Khi người dùng đóng màn hình chính,
                    // xóa phiên đăng nhập và đóng FormLogin
                    SessionManager.Clear();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Sai tài khoản hoặc mật khẩu!",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show(
                    "Không thể kết nối đến API.\n" +
                    "Hãy kiểm tra MiniSupermarket.API có đang chạy " +
                    "và đang lắng nghe tại http://localhost:5000.",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show(
                    "API trả về dữ liệu không đúng định dạng JSON.",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Đã xảy ra lỗi: " + ex.Message,
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}