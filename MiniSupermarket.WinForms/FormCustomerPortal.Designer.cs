namespace MiniSupermarket.WinForms
{
    partial class FormCustomerPortal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblUserGreeting = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlLeaderboardContainer = new System.Windows.Forms.Panel();
            this.dgvLeaderboard = new System.Windows.Forms.DataGridView();
            this.pnlLeaderboardHeader = new System.Windows.Forms.Panel();
            this.btnMyOrders = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblLeaderboardTitle = new System.Windows.Forms.Label();
            this.tblCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMyCard = new System.Windows.Forms.Panel();
            this.lblMyStanding = new System.Windows.Forms.Label();
            this.lblMyPoints = new System.Windows.Forms.Label();
            this.lblMyPointsCaption = new System.Windows.Forms.Label();
            this.lblMyRankBadge = new System.Windows.Forms.Label();
            this.lblMyPhone = new System.Windows.Forms.Label();
            this.lblMyName = new System.Windows.Forms.Label();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.pnlRankRules = new System.Windows.Forms.Panel();
            this.lblPerksList = new System.Windows.Forms.Label();
            this.lblNextRankGoal = new System.Windows.Forms.Label();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.pnlStatsCard = new System.Windows.Forms.Panel();
            this.lblSpendVal = new System.Windows.Forms.Label();
            this.lblSpendCap = new System.Windows.Forms.Label();
            this.lblOrdersVal = new System.Windows.Forms.Label();
            this.lblOrdersCap = new System.Windows.Forms.Label();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlLeaderboardContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeaderboard)).BeginInit();
            this.pnlLeaderboardHeader.SuspendLayout();
            this.tblCards.SuspendLayout();
            this.pnlMyCard.SuspendLayout();
            this.pnlRankRules.SuspendLayout();
            this.pnlStatsCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.pnlTop.Controls.Add(this.btnLogout);
            this.pnlTop.Controls.Add(this.lblUserGreeting);
            this.pnlTop.Controls.Add(this.lblHeaderSubtitle);
            this.pnlTop.Controls.Add(this.lblHeaderTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlTop.Size = new System.Drawing.Size(1200, 70);
            this.pnlTop.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(1075, 17);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(105, 34);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "⏻  Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // lblUserGreeting
            // 
            this.lblUserGreeting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserGreeting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserGreeting.ForeColor = System.Drawing.Color.White;
            this.lblUserGreeting.Location = new System.Drawing.Point(750, 22);
            this.lblUserGreeting.Name = "lblUserGreeting";
            this.lblUserGreeting.Size = new System.Drawing.Size(315, 23);
            this.lblUserGreeting.TabIndex = 2;
            this.lblUserGreeting.Text = "Xin chào: Khách hàng";
            this.lblUserGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(20, 42);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(394, 15);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Bách hóa Tổng hợp Hoàng Dung • Chương trình Khách hàng thân thiết VIP";
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(18, 12);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(465, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "🏆  CỔNG THÀNH VIÊN - BẢNG XẾP HẠNG ĐIỂM";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlLeaderboardContainer);
            this.pnlMain.Controls.Add(this.tblCards);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 70);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(18, 14, 18, 14);
            this.pnlMain.Size = new System.Drawing.Size(1200, 670);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlLeaderboardContainer
            // 
            this.pnlLeaderboardContainer.BackColor = System.Drawing.Color.White;
            this.pnlLeaderboardContainer.Controls.Add(this.dgvLeaderboard);
            this.pnlLeaderboardContainer.Controls.Add(this.pnlLeaderboardHeader);
            this.pnlLeaderboardContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeaderboardContainer.Location = new System.Drawing.Point(18, 204);
            this.pnlLeaderboardContainer.Name = "pnlLeaderboardContainer";
            this.pnlLeaderboardContainer.Padding = new System.Windows.Forms.Padding(12);
            this.pnlLeaderboardContainer.Size = new System.Drawing.Size(1164, 452);
            this.pnlLeaderboardContainer.TabIndex = 1;
            // 
            // dgvLeaderboard
            // 
            this.dgvLeaderboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLeaderboard.Location = new System.Drawing.Point(12, 60);
            this.dgvLeaderboard.Name = "dgvLeaderboard";
            this.dgvLeaderboard.Size = new System.Drawing.Size(1140, 380);
            this.dgvLeaderboard.TabIndex = 1;
            // 
            // pnlLeaderboardHeader
            // 
            this.pnlLeaderboardHeader.Controls.Add(this.btnMyOrders);
            this.pnlLeaderboardHeader.Controls.Add(this.btnRefresh);
            this.pnlLeaderboardHeader.Controls.Add(this.txtSearch);
            this.pnlLeaderboardHeader.Controls.Add(this.lblSearch);
            this.pnlLeaderboardHeader.Controls.Add(this.lblLeaderboardTitle);
            this.pnlLeaderboardHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeaderboardHeader.Location = new System.Drawing.Point(12, 12);
            this.pnlLeaderboardHeader.Name = "pnlLeaderboardHeader";
            this.pnlLeaderboardHeader.Size = new System.Drawing.Size(1140, 48);
            this.pnlLeaderboardHeader.TabIndex = 0;
            // 
            // btnMyOrders
            // 
            this.btnMyOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMyOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(136)))), ((int)(((byte)(209)))));
            this.btnMyOrders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMyOrders.FlatAppearance.BorderSize = 0;
            this.btnMyOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyOrders.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnMyOrders.ForeColor = System.Drawing.Color.White;
            this.btnMyOrders.Location = new System.Drawing.Point(965, 8);
            this.btnMyOrders.Name = "btnMyOrders";
            this.btnMyOrders.Size = new System.Drawing.Size(170, 32);
            this.btnMyOrders.TabIndex = 4;
            this.btnMyOrders.Text = "📜  Lịch sử mua hàng";
            this.btnMyOrders.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(107)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(855, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 32);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄  Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtSearch.Location = new System.Drawing.Point(620, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 26);
            this.txtSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.Gray;
            this.lblSearch.Location = new System.Drawing.Point(552, 16);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(61, 15);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "TÌM KIẾM:";
            // 
            // lblLeaderboardTitle
            // 
            this.lblLeaderboardTitle.AutoSize = true;
            this.lblLeaderboardTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLeaderboardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblLeaderboardTitle.Location = new System.Drawing.Point(4, 12);
            this.lblLeaderboardTitle.Name = "lblLeaderboardTitle";
            this.lblLeaderboardTitle.Size = new System.Drawing.Size(350, 21);
            this.lblLeaderboardTitle.TabIndex = 0;
            this.lblLeaderboardTitle.Text = "🌟  BẢNG XẾP HẠNG ĐIỂM TÍCH LŨY TOÀN HỆ THỐNG";
            // 
            // tblCards
            // 
            this.tblCards.ColumnCount = 3;
            this.tblCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tblCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tblCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tblCards.Controls.Add(this.pnlMyCard, 0, 0);
            this.tblCards.Controls.Add(this.pnlRankRules, 1, 0);
            this.tblCards.Controls.Add(this.pnlStatsCard, 2, 0);
            this.tblCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblCards.Location = new System.Drawing.Point(18, 14);
            this.tblCards.Name = "tblCards";
            this.tblCards.RowCount = 1;
            this.tblCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblCards.Size = new System.Drawing.Size(1164, 190);
            this.tblCards.TabIndex = 0;
            // 
            // pnlMyCard
            // 
            this.pnlMyCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(92)))));
            this.pnlMyCard.Controls.Add(this.lblMyStanding);
            this.pnlMyCard.Controls.Add(this.lblMyPoints);
            this.pnlMyCard.Controls.Add(this.lblMyPointsCaption);
            this.pnlMyCard.Controls.Add(this.lblMyRankBadge);
            this.pnlMyCard.Controls.Add(this.lblMyPhone);
            this.pnlMyCard.Controls.Add(this.lblMyName);
            this.pnlMyCard.Controls.Add(this.lblCard1Title);
            this.pnlMyCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMyCard.Location = new System.Drawing.Point(0, 0);
            this.pnlMyCard.Margin = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.pnlMyCard.Name = "pnlMyCard";
            this.pnlMyCard.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMyCard.Size = new System.Drawing.Size(432, 180);
            this.pnlMyCard.TabIndex = 0;
            // 
            // lblMyStanding
            // 
            this.lblMyStanding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMyStanding.AutoSize = true;
            this.lblMyStanding.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMyStanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(236)))), ((int)(((byte)(179)))));
            this.lblMyStanding.Location = new System.Drawing.Point(16, 145);
            this.lblMyStanding.Name = "lblMyStanding";
            this.lblMyStanding.Size = new System.Drawing.Size(175, 17);
            this.lblMyStanding.TabIndex = 6;
            this.lblMyStanding.Text = "🏆  Vị trí hiện tại: Hạng #1";
            // 
            // lblMyPoints
            // 
            this.lblMyPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMyPoints.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblMyPoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.lblMyPoints.Location = new System.Drawing.Point(210, 68);
            this.lblMyPoints.Name = "lblMyPoints";
            this.lblMyPoints.Size = new System.Drawing.Size(206, 45);
            this.lblMyPoints.TabIndex = 5;
            this.lblMyPoints.Text = "1,500";
            this.lblMyPoints.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMyPointsCaption
            // 
            this.lblMyPointsCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMyPointsCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMyPointsCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
            this.lblMyPointsCaption.Location = new System.Drawing.Point(230, 48);
            this.lblMyPointsCaption.Name = "lblMyPointsCaption";
            this.lblMyPointsCaption.Size = new System.Drawing.Size(186, 18);
            this.lblMyPointsCaption.TabIndex = 4;
            this.lblMyPointsCaption.Text = "ĐIỂM HIỆN CÓ";
            this.lblMyPointsCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMyRankBadge
            // 
            this.lblMyRankBadge.AutoSize = true;
            this.lblMyRankBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblMyRankBadge.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMyRankBadge.ForeColor = System.Drawing.Color.White;
            this.lblMyRankBadge.Location = new System.Drawing.Point(16, 110);
            this.lblMyRankBadge.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.lblMyRankBadge.Name = "lblMyRankBadge";
            this.lblMyRankBadge.Size = new System.Drawing.Size(130, 23);
            this.lblMyRankBadge.TabIndex = 3;
            this.lblMyRankBadge.Text = "💎  KIM CƯƠNG";
            // 
            // lblMyPhone
            // 
            this.lblMyPhone.AutoSize = true;
            this.lblMyPhone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMyPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(235)))), ((int)(((byte)(230)))));
            this.lblMyPhone.Location = new System.Drawing.Point(16, 76);
            this.lblMyPhone.Name = "lblMyPhone";
            this.lblMyPhone.Size = new System.Drawing.Size(100, 17);
            this.lblMyPhone.TabIndex = 2;
            this.lblMyPhone.Text = "📞  0987 654 321";
            // 
            // lblMyName
            // 
            this.lblMyName.AutoSize = true;
            this.lblMyName.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblMyName.ForeColor = System.Drawing.Color.White;
            this.lblMyName.Location = new System.Drawing.Point(14, 46);
            this.lblMyName.Name = "lblMyName";
            this.lblMyName.Size = new System.Drawing.Size(135, 25);
            this.lblMyName.TabIndex = 1;
            this.lblMyName.Text = "Lê Hoàng Nam";
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCard1Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.lblCard1Title.Location = new System.Drawing.Point(16, 14);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(147, 15);
            this.lblCard1Title.TabIndex = 0;
            this.lblCard1Title.Text = "THẺ THÀNH VIÊN CỦA BẠN";
            // 
            // pnlRankRules
            // 
            this.pnlRankRules.BackColor = System.Drawing.Color.White;
            this.pnlRankRules.Controls.Add(this.lblPerksList);
            this.pnlRankRules.Controls.Add(this.lblNextRankGoal);
            this.pnlRankRules.Controls.Add(this.lblCard2Title);
            this.pnlRankRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRankRules.Location = new System.Drawing.Point(442, 0);
            this.pnlRankRules.Margin = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.pnlRankRules.Name = "pnlRankRules";
            this.pnlRankRules.Padding = new System.Windows.Forms.Padding(16);
            this.pnlRankRules.Size = new System.Drawing.Size(385, 180);
            this.pnlRankRules.TabIndex = 1;
            // 
            // lblPerksList
            // 
            this.lblPerksList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPerksList.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPerksList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblPerksList.Location = new System.Drawing.Point(16, 70);
            this.lblPerksList.Name = "lblPerksList";
            this.lblPerksList.Size = new System.Drawing.Size(353, 94);
            this.lblPerksList.TabIndex = 2;
            this.lblPerksList.Text = "💎 Kim Cương (>= 1,000đ): Giảm 5% + Miễn phí giao hàng\r\n🥇 Vàng (500 - 999đ): Gi" +
    "ảm 3% ngày hội thành viên\r\n🥈 Bạc (100 - 499đ): Quà sinh nhật + Tích điểm x1.5\r\n" +
    "🥉 Đồng (0 - 99đ): Tích 1 điểm cho mỗi 10.000 đ";
            // 
            // lblNextRankGoal
            // 
            this.lblNextRankGoal.AutoSize = true;
            this.lblNextRankGoal.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNextRankGoal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(107)))));
            this.lblNextRankGoal.Location = new System.Drawing.Point(14, 40);
            this.lblNextRankGoal.Name = "lblNextRankGoal";
            this.lblNextRankGoal.Size = new System.Drawing.Size(242, 17);
            this.lblNextRankGoal.TabIndex = 1;
            this.lblNextRankGoal.Text = "🎉  Bạn đang ở hạng Kim Cương cao nhất!";
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCard2Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblCard2Title.Location = new System.Drawing.Point(14, 14);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(184, 15);
            this.lblCard2Title.TabIndex = 0;
            this.lblCard2Title.Text = "QUYỀN LỢI & CẤP BẬC THÀNH VIÊN";
            // 
            // pnlStatsCard
            // 
            this.pnlStatsCard.BackColor = System.Drawing.Color.White;
            this.pnlStatsCard.Controls.Add(this.lblSpendVal);
            this.pnlStatsCard.Controls.Add(this.lblSpendCap);
            this.pnlStatsCard.Controls.Add(this.lblOrdersVal);
            this.pnlStatsCard.Controls.Add(this.lblOrdersCap);
            this.pnlStatsCard.Controls.Add(this.lblCard3Title);
            this.pnlStatsCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatsCard.Location = new System.Drawing.Point(837, 0);
            this.pnlStatsCard.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlStatsCard.Name = "pnlStatsCard";
            this.pnlStatsCard.Padding = new System.Windows.Forms.Padding(16);
            this.pnlStatsCard.Size = new System.Drawing.Size(327, 180);
            this.pnlStatsCard.TabIndex = 2;
            // 
            // lblSpendVal
            // 
            this.lblSpendVal.AutoSize = true;
            this.lblSpendVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblSpendVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblSpendVal.Location = new System.Drawing.Point(14, 130);
            this.lblSpendVal.Name = "lblSpendVal";
            this.lblSpendVal.Size = new System.Drawing.Size(117, 30);
            this.lblSpendVal.TabIndex = 4;
            this.lblSpendVal.Text = "450,000 đ";
            // 
            // lblSpendCap
            // 
            this.lblSpendCap.AutoSize = true;
            this.lblSpendCap.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblSpendCap.ForeColor = System.Drawing.Color.Gray;
            this.lblSpendCap.Location = new System.Drawing.Point(14, 110);
            this.lblSpendCap.Name = "lblSpendCap";
            this.lblSpendCap.Size = new System.Drawing.Size(91, 15);
            this.lblSpendCap.TabIndex = 3;
            this.lblSpendCap.Text = "TỔNG CHI TIÊU";
            // 
            // lblOrdersVal
            // 
            this.lblOrdersVal.AutoSize = true;
            this.lblOrdersVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblOrdersVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(107)))));
            this.lblOrdersVal.Location = new System.Drawing.Point(14, 66);
            this.lblOrdersVal.Name = "lblOrdersVal";
            this.lblOrdersVal.Size = new System.Drawing.Size(78, 30);
            this.lblOrdersVal.TabIndex = 2;
            this.lblOrdersVal.Text = "3 Đơn";
            // 
            // lblOrdersCap
            // 
            this.lblOrdersCap.AutoSize = true;
            this.lblOrdersCap.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblOrdersCap.ForeColor = System.Drawing.Color.Gray;
            this.lblOrdersCap.Location = new System.Drawing.Point(14, 46);
            this.lblOrdersCap.Name = "lblOrdersCap";
            this.lblOrdersCap.Size = new System.Drawing.Size(110, 15);
            this.lblOrdersCap.TabIndex = 1;
            this.lblOrdersCap.Text = "SỐ ĐƠN ĐÃ MUA";
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCard3Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblCard3Title.Location = new System.Drawing.Point(14, 14);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(130, 15);
            this.lblCard3Title.TabIndex = 0;
            this.lblCard3Title.Text = "HOẠT ĐỘNG CỦA BẠN";
            // 
            // FormCustomerPortal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1200, 740);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "FormCustomerPortal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cổng Khách Hàng - Bảng Xếp Hạng Điểm Tích Lũy • Bách Hóa Hoàng Dung";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlLeaderboardContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeaderboard)).EndInit();
            this.pnlLeaderboardHeader.ResumeLayout(false);
            this.pnlLeaderboardHeader.PerformLayout();
            this.tblCards.ResumeLayout(false);
            this.pnlMyCard.ResumeLayout(false);
            this.pnlMyCard.PerformLayout();
            this.pnlRankRules.ResumeLayout(false);
            this.pnlRankRules.PerformLayout();
            this.pnlStatsCard.ResumeLayout(false);
            this.pnlStatsCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblUserGreeting;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tblCards;
        private System.Windows.Forms.Panel pnlMyCard;
        private System.Windows.Forms.Label lblCard1Title;
        private System.Windows.Forms.Label lblMyName;
        private System.Windows.Forms.Label lblMyPhone;
        private System.Windows.Forms.Label lblMyRankBadge;
        private System.Windows.Forms.Label lblMyPointsCaption;
        private System.Windows.Forms.Label lblMyPoints;
        private System.Windows.Forms.Label lblMyStanding;
        private System.Windows.Forms.Panel pnlRankRules;
        private System.Windows.Forms.Label lblCard2Title;
        private System.Windows.Forms.Label lblNextRankGoal;
        private System.Windows.Forms.Label lblPerksList;
        private System.Windows.Forms.Panel pnlStatsCard;
        private System.Windows.Forms.Label lblCard3Title;
        private System.Windows.Forms.Label lblOrdersCap;
        private System.Windows.Forms.Label lblOrdersVal;
        private System.Windows.Forms.Label lblSpendCap;
        private System.Windows.Forms.Label lblSpendVal;
        private System.Windows.Forms.Panel pnlLeaderboardContainer;
        private System.Windows.Forms.Panel pnlLeaderboardHeader;
        private System.Windows.Forms.Label lblLeaderboardTitle;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnMyOrders;
        private System.Windows.Forms.DataGridView dgvLeaderboard;
    }
}