using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public static class SessionManager
    {
        public static string JwtToken { get; set; } = string.Empty;
        public static string CurrentUsername { get; set; } = string.Empty;
        public static string CurrentFullName { get; set; } = string.Empty;
        public static string CurrentRole { get; set; } = string.Empty;
        public static int? CurrentCustomerId { get; set; }
        public static string CurrentPhone { get; set; } = string.Empty;
        public static DateTime LoginTime { get; set; } = DateTime.Now;

        public static void Clear()
        {
            JwtToken = string.Empty;
            CurrentUsername = string.Empty;
            CurrentFullName = string.Empty;
            CurrentRole = string.Empty;
            CurrentCustomerId = null;
            CurrentPhone = string.Empty;
            LoginTime = DateTime.Now;

            ApiClientService.Client.DefaultRequestHeaders.Authorization = null;
        }
    }

    public static class ApiClientService
    {
        private static readonly HttpClient _client;

        static ApiClientService()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol =
                    System.Net.SecurityProtocolType.Tls12 |
                    System.Net.SecurityProtocolType.Tls11 |
                    System.Net.SecurityProtocolType.Tls;

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (sender, certificate, chain, sslPolicyErrors) => true;

                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => true
                };

                _client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("http://localhost:5000/api/"),
                    Timeout = TimeSpan.FromSeconds(30)
                };
            }
            catch
            {
                _client = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5000/api/"),
                    Timeout = TimeSpan.FromSeconds(30)
                };
            }
        }

        // Client dùng chung cho toàn bộ Form
        public static HttpClient Client
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SessionManager.JwtToken))
                {
                    _client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            SessionManager.JwtToken);
                }
                else
                {
                    _client.DefaultRequestHeaders.Authorization = null;
                }

                return _client;
            }
        }

        // Đăng nhập và lấy JWT Token
        public static async Task<bool> LoginAsync(
            string username,
            string password)
        {
            try
            {
                var loginObj = new
                {
                    Username = username,
                    Password = password
                };

                var response = await _client.PostAsJsonAsync(
                    "auth/login",
                    loginObj);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                string jsonString =
                    await response.Content.ReadAsStringAsync();

                JsonDocument doc = JsonDocument.Parse(jsonString);

                JsonElement root = doc.RootElement;

                SessionManager.JwtToken =
                    root.GetProperty("token").GetString()
                    ?? string.Empty;

                SessionManager.CurrentRole =
                    root.GetProperty("role").GetString()
                    ?? string.Empty;

                SessionManager.CurrentUsername = username;

                return !string.IsNullOrWhiteSpace(
                    SessionManager.JwtToken);
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        // Gọi API lấy dữ liệu có JWT Token
        public static async Task<string> GetDataWithTokenAsync(
            string endpoint)
        {
            HttpResponseMessage response =
                await Client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception(
                    "Phiên làm việc hết hạn hoặc chưa đăng nhập!");
            }

            string error =
                await response.Content.ReadAsStringAsync();

            throw new Exception(
                $"Lỗi khi gọi API: {(int)response.StatusCode} " +
                $"{response.StatusCode}\n{error}");
        }
    }

    public static class WinFormsTheme
    {
        public static readonly Color Primary = Color.FromArgb(0, 121, 107);
        public static readonly Color PrimaryDark = Color.FromArgb(0, 77, 64);
        public static readonly Color Surface = Color.White;
        public static readonly Color PageBackground = Color.FromArgb(245, 247, 246);
        public static readonly Color Border = Color.FromArgb(220, 228, 226);
        public static readonly Color SelectedRow = Color.FromArgb(178, 223, 219);

        public static void Apply(Form form)
        {
            form.Font = new Font("Segoe UI", 10F);
            form.BackColor = PageBackground;
            form.ForeColor = Color.FromArgb(35, 45, 45);
            form.AutoScaleMode = AutoScaleMode.Font;
            form.ClientSize = new Size(1280, 720);
            form.MinimumSize = new Size(1000, 600);
            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.UseVisualStyleBackColor = false;
                    button.BackColor = Primary;
                    button.ForeColor = Color.White;
                    button.Cursor = Cursors.Hand;
                    button.MinimumSize = new Size(110, 36);
                }

                TextBox textBox = control as TextBox;
                if (textBox != null)
                {
                    textBox.Font = new Font("Segoe UI", 10F);
                    textBox.BackColor = Surface;
                    textBox.ForeColor = Color.FromArgb(35, 45, 45);
                }

                ComboBox comboBox = control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.Font = new Font("Segoe UI", 10F);
                    comboBox.BackColor = Surface;
                    comboBox.ForeColor = Color.FromArgb(35, 45, 45);
                }

                DataGridView grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Surface;
                    grid.BorderStyle = BorderStyle.None;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryDark;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    grid.ColumnHeadersHeight = 36;
                    grid.RowTemplate.Height = 32;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 250, 249);
                    grid.DefaultCellStyle.SelectionBackColor = SelectedRow;
                    grid.DefaultCellStyle.SelectionForeColor = Color.Black;
                    grid.GridColor = Border;
                    grid.RowHeadersVisible = false;
                    grid.AllowUserToResizeRows = false;
                }

                ApplyToControls(control.Controls);
            }
        }
    }
}
