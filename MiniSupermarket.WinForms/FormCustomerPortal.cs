using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCustomerPortal : Form
    {
        private List<LeaderboardItemDto> _allMembers = new List<LeaderboardItemDto>();
        private LeaderboardItemDto _currentCustomer = null;

        public FormCustomerPortal()
        {
            InitializeComponent();
            SetupGrid();
            WireEvents();
        }

        private void SetupGrid()
        {
            dgvLeaderboard.AutoGenerateColumns = false;
            dgvLeaderboard.AllowUserToAddRows = false;
            dgvLeaderboard.AllowUserToDeleteRows = false;
            dgvLeaderboard.ReadOnly = true;
            dgvLeaderboard.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLeaderboard.MultiSelect = false;

            dgvLeaderboard.BackgroundColor = Color.White;
            dgvLeaderboard.BorderStyle = BorderStyle.None;
            dgvLeaderboard.RowHeadersVisible = false;
            dgvLeaderboard.EnableHeadersVisualStyles = false;
            dgvLeaderboard.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
            dgvLeaderboard.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLeaderboard.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvLeaderboard.ColumnHeadersHeight = 38;
            dgvLeaderboard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLeaderboard.RowTemplate.Height = 34;
            dgvLeaderboard.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 251, 250);
            dgvLeaderboard.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgvLeaderboard.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLeaderboard.GridColor = Color.FromArgb(224, 224, 224);
            dgvLeaderboard.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLeaderboard.AllowUserToResizeRows = false;

            dgvLeaderboard.Columns.Clear();

            dgvLeaderboard.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LeaderboardItemDto.RankDisplay),
                HeaderText = "Hạng",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                }
            });

            dgvLeaderboard.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LeaderboardItemDto.FullName),
                HeaderText = "Họ và tên khách hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                }
            });

            dgvLeaderboard.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LeaderboardItemDto.MaskedPhone),
                HeaderText = "Số điện thoại",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvLeaderboard.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LeaderboardItemDto.MembershipRank),
                HeaderText = "Hạng thành viên",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                }
            });

            dgvLeaderboard.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LeaderboardItemDto.PointsFormatted),
                HeaderText = "Điểm tích lũy",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 105, 92)
                }
            });

            dgvLeaderboard.CellFormatting += dgvLeaderboard_CellFormatting;
        }

        private void WireEvents()
        {
            this.Load += FormCustomerPortal_Load;
            btnRefresh.Click += async (s, e) => await LoadLeaderboardAsync();
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            btnLogout.Click += btnLogout_Click;
            btnMyOrders.Click += btnMyOrders_Click;
        }

        private async void FormCustomerPortal_Load(object sender, EventArgs e)
        {
            string displayName = !string.IsNullOrWhiteSpace(SessionManager.CurrentFullName)
                ? SessionManager.CurrentFullName
                : SessionManager.CurrentUsername;

            lblUserGreeting.Text = $"Xin chào: {displayName}";
            lblMyName.Text = displayName;
            lblMyPhone.Text = "📞  " + (!string.IsNullOrWhiteSpace(SessionManager.CurrentPhone) ? SessionManager.CurrentPhone : SessionManager.CurrentUsername);

            await LoadLeaderboardAsync();
            await LoadMyStatsAsync();
        }

        private async Task LoadLeaderboardAsync()
        {
            try
            {
                btnRefresh.Enabled = false;
                btnRefresh.Text = "Đang tải...";

                List<LeaderboardItemDto> list = null;

                try
                {
                    list = await ApiClientService.Client
                        .GetFromJsonAsync<List<LeaderboardItemDto>>("customers/leaderboard");
                }
                catch
                {
                    // Fallback to customers list
                    var rawList = await ApiClientService.Client
                        .GetFromJsonAsync<List<CustomerDto>>("customers");

                    if (rawList != null)
                    {
                        var sorted = rawList.OrderByDescending(c => c.Points).ToList();
                        int r = 1;
                        list = sorted.Select(c => new LeaderboardItemDto
                        {
                            Rank = r++,
                            CustomerId = c.CustomerId,
                            FullName = c.FullName,
                            PhoneNumber = c.Phone,
                            Points = c.Points,
                            MembershipRank = c.MembershipRank
                        }).ToList();
                    }
                }

                _allMembers = list ?? new List<LeaderboardItemDto>();

                // Xác định khách hàng hiện tại
                ResolveCurrentCustomer();

                ApplyFilter();
                UpdateMyCardDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải bảng xếp hạng:\n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnRefresh.Text = "🔄  Làm mới";
                btnRefresh.Enabled = true;
            }
        }

        private void ResolveCurrentCustomer()
        {
            if (_allMembers.Count == 0) return;

            string myUsername = (SessionManager.CurrentUsername ?? string.Empty).Trim().ToLower();
            string myPhone = (SessionManager.CurrentPhone ?? string.Empty).Trim();
            string myName = (SessionManager.CurrentFullName ?? string.Empty).Trim().ToLower();
            int? myId = SessionManager.CurrentCustomerId;

            if (myId.HasValue && myId.Value > 0)
            {
                _currentCustomer = _allMembers.FirstOrDefault(m => m.CustomerId == myId.Value);
            }

            if (_currentCustomer == null && !string.IsNullOrWhiteSpace(myPhone))
            {
                _currentCustomer = _allMembers.FirstOrDefault(m => m.PhoneNumber == myPhone);
            }

            if (_currentCustomer == null && !string.IsNullOrWhiteSpace(myUsername))
            {
                _currentCustomer = _allMembers.FirstOrDefault(m =>
                    m.PhoneNumber == myUsername ||
                    (m.FullName != null && m.FullName.ToLower() == myUsername));
            }

            if (_currentCustomer == null && !string.IsNullOrWhiteSpace(myName))
            {
                _currentCustomer = _allMembers.FirstOrDefault(m =>
                    m.FullName != null && m.FullName.ToLower() == myName);
            }

            // Fallback: nếu chưa tìm thấy, chọn phần tử đầu tiên hoặc tạo mẫu
            if (_currentCustomer == null)
            {
                _currentCustomer = _allMembers[0];
            }
        }

        private void UpdateMyCardDisplay()
        {
            if (_currentCustomer == null) return;

            lblMyName.Text = _currentCustomer.FullName;
            lblMyPhone.Text = "📞  " + _currentCustomer.PhoneNumber;
            lblMyPoints.Text = _currentCustomer.Points.ToString("N0");

            string rank = string.IsNullOrEmpty(_currentCustomer.MembershipRank) ? "Đồng" : _currentCustomer.MembershipRank;
            lblMyRankBadge.Text = GetRankBadgeText(rank);

            lblMyStanding.Text = $"🏆  Vị trí hiện tại: Hạng #{_currentCustomer.Rank} toàn hệ thống";

            // Tính điểm cần để lên hạng tiếp theo
            int pts = _currentCustomer.Points;
            if (pts >= 1000)
            {
                lblNextRankGoal.Text = "🎉  Bạn đang ở hạng Kim Cương cao nhất!";
                lblNextRankGoal.ForeColor = Color.FromArgb(0, 121, 107);
            }
            else if (pts >= 500)
            {
                int needed = 1000 - pts;
                lblNextRankGoal.Text = $"🚀  Cần thêm {needed:N0} điểm để thăng hạng Kim Cương!";
                lblNextRankGoal.ForeColor = Color.FromArgb(2, 136, 209);
            }
            else if (pts >= 100)
            {
                int needed = 500 - pts;
                lblNextRankGoal.Text = $"🚀  Cần thêm {needed:N0} điểm để thăng hạng Vàng!";
                lblNextRankGoal.ForeColor = Color.FromArgb(245, 124, 0);
            }
            else
            {
                int needed = 100 - pts;
                lblNextRankGoal.Text = $"🚀  Cần thêm {needed:N0} điểm để thăng hạng Bạc!";
                lblNextRankGoal.ForeColor = Color.FromArgb(120, 144, 156);
            }
        }

        private string GetRankBadgeText(string rank)
        {
            switch (rank.Trim().ToLower())
            {
                case "kim cương": return "💎  HẠNG KIM CƯƠNG";
                case "vàng": return "🥇  HẠNG VÀNG";
                case "bạc": return "🥈  HẠNG BẠC";
                default: return "🥉  HẠNG ĐỒNG";
            }
        }

        private async Task LoadMyStatsAsync()
        {
            if (_currentCustomer == null) return;

            try
            {
                var orders = await ApiClientService.Client
                    .GetFromJsonAsync<List<OrderHistoryDto>>($"customers/{_currentCustomer.CustomerId}/orders");

                if (orders != null && orders.Count > 0)
                {
                    lblOrdersVal.Text = $"{orders.Count} Đơn";
                    decimal totalSpend = orders.Sum(o => o.TotalAmount);
                    lblSpendVal.Text = $"{totalSpend:N0} đ";
                }
                else
                {
                    lblOrdersVal.Text = "0 Đơn";
                    lblSpendVal.Text = "0 đ";
                }
            }
            catch
            {
                lblOrdersVal.Text = "—";
                lblSpendVal.Text = "—";
            }
        }

        private void ApplyFilter()
        {
            string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLower();

            var query = _allMembers.AsEnumerable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m =>
                    (m.FullName ?? "").ToLower().Contains(keyword) ||
                    (m.PhoneNumber ?? "").Contains(keyword) ||
                    (m.MembershipRank ?? "").ToLower().Contains(keyword) ||
                    m.Rank.ToString() == keyword);
            }

            var result = query.ToList();
            dgvLeaderboard.DataSource = result;

            // Highlight dòng của khách hiện tại
            if (_currentCustomer != null)
            {
                foreach (DataGridViewRow row in dgvLeaderboard.Rows)
                {
                    if (row.DataBoundItem is LeaderboardItemDto item && item.CustomerId == _currentCustomer.CustomerId)
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        private void dgvLeaderboard_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLeaderboard.Rows.Count) return;

            if (dgvLeaderboard.Rows[e.RowIndex].DataBoundItem is LeaderboardItemDto item)
            {
                // Highlight khách hiện tại
                if (_currentCustomer != null && item.CustomerId == _currentCustomer.CustomerId)
                {
                    dgvLeaderboard.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(224, 242, 241); // Light Teal
                    dgvLeaderboard.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    return;
                }

                // Top 1, 2, 3 huy chương
                if (item.Rank == 1)
                {
                    dgvLeaderboard.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 225); // Gold
                }
                else if (item.Rank == 2)
                {
                    dgvLeaderboard.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Silver
                }
                else if (item.Rank == 3)
                {
                    dgvLeaderboard.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 235); // Bronze
                }
            }
        }

        private async void btnMyOrders_Click(object sender, EventArgs e)
        {
            if (_currentCustomer == null)
            {
                MessageBox.Show("Không tìm thấy thông tin khách hàng hiện tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var orders = await ApiClientService.Client
                    .GetFromJsonAsync<List<OrderHistoryDto>>($"customers/{_currentCustomer.CustomerId}/orders");

                if (orders == null || orders.Count == 0)
                {
                    MessageBox.Show("Bạn chưa có đơn hàng nào trong hệ thống.", "Lịch sử mua hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị hộp thoại chi tiết đơn hàng
                using (Form orderHistoryDialog = new Form())
                {
                    orderHistoryDialog.Text = $"Lịch sử đơn hàng của {_currentCustomer.FullName}";
                    orderHistoryDialog.Size = new Size(780, 480);
                    orderHistoryDialog.StartPosition = FormStartPosition.CenterParent;
                    orderHistoryDialog.Font = new Font("Segoe UI", 9.5F);

                    var grid = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        AutoGenerateColumns = false,
                        AllowUserToAddRows = false,
                        AllowUserToDeleteRows = false,
                        ReadOnly = true,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        BackgroundColor = Color.White,
                        BorderStyle = BorderStyle.None,
                        RowHeadersVisible = false,
                        EnableHeadersVisualStyles = false,
                        RowTemplate = { Height = 32 }
                    };

                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 77, 64);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    grid.ColumnHeadersHeight = 36;

                    grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OrderId", HeaderText = "Mã đơn", Width = 90 });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OrderDate", HeaderText = "Ngày giờ", Width = 160, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CashierUsername", HeaderText = "Thu ngân", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ItemCount", HeaderText = "Số lượng", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalAmount", HeaderText = "Tổng tiền", Width = 140, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 10F, FontStyle.Bold) } });

                    grid.DataSource = orders;

                    var topPanel = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(240, 244, 243), Padding = new Padding(12) };
                    var lblSum = new Label
                    {
                        Dock = DockStyle.Fill,
                        Text = $"Tổng cộng: {orders.Count} đơn  •  Tổng chi tiêu: {orders.Sum(o => o.TotalAmount):N0} đ",
                        Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(0, 77, 64)
                    };
                    topPanel.Controls.Add(lblSum);

                    orderHistoryDialog.Controls.Add(grid);
                    orderHistoryDialog.Controls.Add(topPanel);

                    orderHistoryDialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SessionManager.Clear();
                this.Close();
            }
        }
    }

    public class LeaderboardItemDto
    {
        public int Rank { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MaskedPhone
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length < 7)
                    return PhoneNumber ?? string.Empty;

                return PhoneNumber.Substring(0, 3) + "****" + PhoneNumber.Substring(PhoneNumber.Length - 3);
            }
        }
        public int Points { get; set; }
        public string MembershipRank { get; set; } = "Đồng";

        public string PointsFormatted => Points.ToString("N0") + " đ";

        public string RankDisplay
        {
            get
            {
                switch (Rank)
                {
                    case 1: return "Top 1 🥇";
                    case 2: return "Top 2 🥈";
                    case 3: return "Top 3 🥉";
                    default: return $"#{Rank}";
                }
            }
        }
    }
}