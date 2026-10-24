using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            this.Load += FormSettings_Load;
            btnCheckConnection.Click += async (s, e) => await CheckApiConnectionAsync();
        }

        private async void FormSettings_Load(object sender, EventArgs e)
        {
            BindSessionData();
            await CheckApiConnectionAsync();
        }

        private void BindSessionData()
        {
            string username = string.IsNullOrWhiteSpace(SessionManager.CurrentUsername) ? "admin" : SessionManager.CurrentUsername;
            string role = string.IsNullOrWhiteSpace(SessionManager.CurrentRole) ? "Admin" : SessionManager.CurrentRole;
            string fullName = string.IsNullOrWhiteSpace(SessionManager.CurrentFullName) ? "Chủ Web" : SessionManager.CurrentFullName;

            valUsername.Text = username;
            valFullName.Text = fullName;
            valRole.Text = role;
            valLoginTime.Text = SessionManager.LoginTime.ToString("HH:mm:ss dd/MM/yyyy");

            lblHeaderUserInfo.Text = $"Nhân viên: {username}  |  Vai trò: {role}";

            string apiUrl = ApiClientService.Client?.BaseAddress?.ToString() ?? "http://localhost:5000/api/";
            valApiUrl.Text = apiUrl;
        }

        private async Task CheckApiConnectionAsync()
        {
            btnCheckConnection.Enabled = false;
            btnCheckConnection.Text = "Đang kiểm tra...";
            lblStatusIndicator.ForeColor = Color.FromArgb(245, 124, 0); // Orange
            lblStatusIndicator.Text = "● Đang kết nối tới máy chủ API...";

            try
            {
                using (var response = await ApiClientService.Client.GetAsync("Categories"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        lblStatusIndicator.ForeColor = Color.FromArgb(46, 125, 50); // Green
                        lblStatusIndicator.Text = "● Kết nối API thành công";
                    }
                    else
                    {
                        lblStatusIndicator.ForeColor = Color.FromArgb(211, 47, 47); // Red
                        lblStatusIndicator.Text = $"● Kết nối API thất bại (HTTP {(int)response.StatusCode})";
                    }
                }
            }
            catch (Exception)
            {
                lblStatusIndicator.ForeColor = Color.FromArgb(211, 47, 47); // Red
                lblStatusIndicator.Text = "● Không thể kết nối tới API";
            }
            finally
            {
                lblCheckTimestamp.Text = $"Kiểm tra lúc {DateTime.Now:HH:mm:ss dd/MM/yyyy}";
                btnCheckConnection.Text = "Kiểm tra kết nối";
                btnCheckConnection.Enabled = true;
            }
        }
    }
}