using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormUserManage : Form
    {
        private static readonly string[] Roles = { "Admin", "Cashier", "Warehouse" };
        private const string DefaultRole = "Cashier";

        private const int MinUsernameLength = 3;
        private const int MinPasswordLength = 6;

        private List<UserDto> _users = new List<UserDto>();
        private bool _isLoaded = false;

        public FormUserManage()
        {
            InitializeComponent();
            SetupGrid();

            cboRole.Items.AddRange(Roles);
            cboRole.SelectedItem = DefaultRole;

            WireEvents();

            WinFormsTheme.Apply(this);
        }

        private void WireEvents()
        {
            this.Load += FormUserManage_Load;
            this.VisibleChanged += FormUserManage_VisibleChanged;
            this.btnAddUser.Click += btnAddUser_Click;
            this.btnRefresh.Click += btnRefresh_Click;
            this.btnClear.Click += btnClear_Click;
            this.dgvUsers.CellClick += dgvUsers_CellClick;
        }

        // =====================================================
        // 1. CẤU HÌNH BẢNG
        // =====================================================
        private void SetupGrid()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserDto.Id),
                HeaderText = "Mã",
                FillWeight = 8
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserDto.Username),
                HeaderText = "Tên đăng nhập",
                FillWeight = 28
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserDto.FullName),
                HeaderText = "Họ và tên",
                FillWeight = 34
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserDto.Role),
                HeaderText = "Vai trò",
                FillWeight = 16
            });
            dgvUsers.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(UserDto.IsActive),
                HeaderText = "Hoạt động",
                FillWeight = 14
            });
        }

        // =====================================================
        // 2. TẢI DANH SÁCH TÀI KHOẢN
        // =====================================================
        private async void FormUserManage_Load(object sender, EventArgs e)
        {
            if (_isLoaded) return;
            _isLoaded = true;
            await LoadUsersAsync();
            txtUsername.Focus();
        }

        private async void FormUserManage_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !_isLoaded)
            {
                _isLoaded = true;
                await LoadUsersAsync();
                txtUsername.Focus();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            SetStatus("Đang tải danh sách tài khoản...", false);

            try
            {
                var users = await ApiClientService.Client.GetFromJsonAsync<List<UserDto>>("users");

                _users = users ?? new List<UserDto>();
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = _users;

                SetStatus($"Đã tải {_users.Count} tài khoản.", false);
            }
            catch (HttpRequestException ex)
            {
                SetStatus("Không thể kết nối API.", true);
                MessageBox.Show(
                    "Không thể kết nối API để lấy danh sách tài khoản.\n\n" + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                SetStatus("Yêu cầu hết thời gian chờ.", true);
                MessageBox.Show(
                    "Yêu cầu đã hết thời gian chờ. Vui lòng thử lại!",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                SetStatus("Lỗi lấy danh sách tài khoản.", true);
                MessageBox.Show(
                    "Lỗi lấy danh sách tài khoản: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // 3. TẠO TÀI KHOẢN
        // =====================================================
        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;          // KHÔNG Trim: mật khẩu phải giữ nguyên
            string fullName = txtFullName.Text.Trim();
            string role = cboRole.SelectedItem?.ToString() ?? DefaultRole;

            if (!ValidateInput(username, password))
                return;

            var newUser = new
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Role = role
            };

            SetBusy(true);
            try
            {
                HttpResponseMessage res =
                    await ApiClientService.Client.PostAsJsonAsync("users", newUser);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tạo tài khoản mới thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearInput();
                    await LoadUsersAsync();
                }
                else
                {
                    await ShowCreateFailedAsync(res);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    "Không thể kết nối máy chủ để tạo tài khoản.\n\n" + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show(
                    "Yêu cầu tạo tài khoản hết thời gian chờ!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Đã xảy ra lỗi khi tạo tài khoản.\n\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        // =====================================================
        // 4. HÀM HỖ TRỢ
        // =====================================================
        private bool ValidateInput(string username, string password)
        {
            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không được trống!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                (username.Length == 0 ? txtUsername : txtPassword).Focus();
                return false;
            }

            if (username.Length < MinUsernameLength || username.Any(char.IsWhiteSpace))
            {
                MessageBox.Show(
                    $"Tên đăng nhập phải từ {MinUsernameLength} ký tự và không chứa khoảng trắng!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                txtUsername.SelectAll();
                return false;
            }

            if (password.Length < MinPasswordLength)
            {
                MessageBox.Show(
                    $"Mật khẩu phải có ít nhất {MinPasswordLength} ký tự!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                txtPassword.SelectAll();
                return false;
            }

            bool exists = _users.Any(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                txtUsername.SelectAll();
                return false;
            }

            return true;
        }

        private async Task ShowCreateFailedAsync(HttpResponseMessage res)
        {
            string detail = await res.Content.ReadAsStringAsync();

            switch (res.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    txtUsername.SelectAll();
                    break;

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    MessageBox.Show("Bạn không có quyền tạo tài khoản!", "Từ chối truy cập",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                default:
                    MessageBox.Show(
                        "Tạo tài khoản thất bại!\n" +
                        $"Mã trạng thái: {(int)res.StatusCode} ({res.StatusCode})\n" +
                        $"Chi tiết: {detail}",
                        "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void ClearInput()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cboRole.SelectedItem = DefaultRole;
            txtUsername.Focus();
        }

        private void SetBusy(bool busy)
        {
            UseWaitCursor = busy;
            btnAddUser.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            btnClear.Enabled = !busy;
        }

        private void SetStatus(string message, bool isError)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = isError ? Color.Firebrick : Color.DimGray;
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _users.Count)
                return;

            var user = _users[e.RowIndex];
            txtUsername.Text = user.Username;
            txtFullName.Text = user.FullName;
            txtPassword.Clear();

            string targetRole = user.Role;
            if (string.Equals(targetRole, "Thu Ngân", StringComparison.OrdinalIgnoreCase)) targetRole = "Cashier";
            if (string.Equals(targetRole, "Kho", StringComparison.OrdinalIgnoreCase) || string.Equals(targetRole, "Thủ Kho", StringComparison.OrdinalIgnoreCase)) targetRole = "Warehouse";

            for (int i = 0; i < cboRole.Items.Count; i++)
            {
                if (string.Equals(cboRole.Items[i].ToString(), targetRole, StringComparison.OrdinalIgnoreCase))
                {
                    cboRole.SelectedIndex = i;
                    break;
                }
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
