namespace MiniSupermarket.WinForms
{
    partial class FormSettings
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
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblHeaderMainTitle = new System.Windows.Forms.Label();
            this.lblHeaderUserInfo = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tblCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCardSession = new System.Windows.Forms.Panel();
            this.lblSessionTitle = new System.Windows.Forms.Label();
            this.lblCapUsername = new System.Windows.Forms.Label();
            this.valUsername = new System.Windows.Forms.Label();
            this.lblCapFullName = new System.Windows.Forms.Label();
            this.valFullName = new System.Windows.Forms.Label();
            this.lblCapRole = new System.Windows.Forms.Label();
            this.valRole = new System.Windows.Forms.Label();
            this.lblCapLoginTime = new System.Windows.Forms.Label();
            this.valLoginTime = new System.Windows.Forms.Label();
            this.pnlCardService = new System.Windows.Forms.Panel();
            this.lblServiceTitle = new System.Windows.Forms.Label();
            this.lblCapApiUrl = new System.Windows.Forms.Label();
            this.valApiUrl = new System.Windows.Forms.Label();
            this.lblStatusIndicator = new System.Windows.Forms.Label();
            this.lblCheckTimestamp = new System.Windows.Forms.Label();
            this.btnCheckConnection = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblFooterNote = new System.Windows.Forms.Label();
            this.pnlTopBar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tblCards.SuspendLayout();
            this.pnlCardSession.SuspendLayout();
            this.pnlCardService.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.White;
            this.pnlTopBar.Controls.Add(this.lblHeaderUserInfo);
            this.pnlTopBar.Controls.Add(this.lblHeaderSubtitle);
            this.pnlTopBar.Controls.Add(this.lblHeaderMainTitle);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.pnlTopBar.Size = new System.Drawing.Size(1040, 85);
            this.pnlTopBar.TabIndex = 0;
            // 
            // lblHeaderUserInfo
            // 
            this.lblHeaderUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeaderUserInfo.AutoSize = true;
            this.lblHeaderUserInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.lblHeaderUserInfo.Location = new System.Drawing.Point(780, 22);
            this.lblHeaderUserInfo.Name = "lblHeaderUserInfo";
            this.lblHeaderUserInfo.Size = new System.Drawing.Size(230, 15);
            this.lblHeaderUserInfo.TabIndex = 2;
            this.lblHeaderUserInfo.Text = "Nhân viên: admin  |  Vai trò: Admin";
            this.lblHeaderUserInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(24, 50);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(395, 17);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Kiểm tra kết nối API và thông tin phiên làm việc đang hoạt động";
            // 
            // lblHeaderMainTitle
            // 
            this.lblHeaderMainTitle.AutoSize = true;
            this.lblHeaderMainTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderMainTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblHeaderMainTitle.Location = new System.Drawing.Point(23, 16);
            this.lblHeaderMainTitle.Name = "lblHeaderMainTitle";
            this.lblHeaderMainTitle.Size = new System.Drawing.Size(222, 30);
            this.lblHeaderMainTitle.TabIndex = 0;
            this.lblHeaderMainTitle.Text = "CÀI ĐẶT HỆ THỐNG";
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Controls.Add(this.tblCards);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 85);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 20, 24, 10);
            this.pnlContent.Size = new System.Drawing.Size(1040, 480);
            this.pnlContent.TabIndex = 1;
            // 
            // tblCards
            // 
            this.tblCards.ColumnCount = 2;
            this.tblCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCards.Controls.Add(this.pnlCardSession, 0, 0);
            this.tblCards.Controls.Add(this.pnlCardService, 1, 0);
            this.tblCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblCards.Location = new System.Drawing.Point(24, 20);
            this.tblCards.Name = "tblCards";
            this.tblCards.RowCount = 1;
            this.tblCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblCards.Size = new System.Drawing.Size(992, 320);
            this.tblCards.TabIndex = 0;
            // 
            // pnlCardSession
            // 
            this.pnlCardSession.BackColor = System.Drawing.Color.White;
            this.pnlCardSession.Controls.Add(this.valLoginTime);
            this.pnlCardSession.Controls.Add(this.lblCapLoginTime);
            this.pnlCardSession.Controls.Add(this.valRole);
            this.pnlCardSession.Controls.Add(this.lblCapRole);
            this.pnlCardSession.Controls.Add(this.valFullName);
            this.pnlCardSession.Controls.Add(this.lblCapFullName);
            this.pnlCardSession.Controls.Add(this.valUsername);
            this.pnlCardSession.Controls.Add(this.lblCapUsername);
            this.pnlCardSession.Controls.Add(this.lblSessionTitle);
            this.pnlCardSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardSession.Location = new System.Drawing.Point(0, 0);
            this.pnlCardSession.Margin = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.pnlCardSession.Name = "pnlCardSession";
            this.pnlCardSession.Padding = new System.Windows.Forms.Padding(20);
            this.pnlCardSession.Size = new System.Drawing.Size(482, 320);
            this.pnlCardSession.TabIndex = 0;
            // 
            // lblSessionTitle
            // 
            this.lblSessionTitle.AutoSize = true;
            this.lblSessionTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSessionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblSessionTitle.Location = new System.Drawing.Point(18, 18);
            this.lblSessionTitle.Name = "lblSessionTitle";
            this.lblSessionTitle.Size = new System.Drawing.Size(155, 20);
            this.lblSessionTitle.TabIndex = 0;
            this.lblSessionTitle.Text = "PHIÊN ĐĂNG NHẬP";
            // 
            // lblCapUsername
            // 
            this.lblCapUsername.AutoSize = true;
            this.lblCapUsername.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblCapUsername.Location = new System.Drawing.Point(20, 58);
            this.lblCapUsername.Name = "lblCapUsername";
            this.lblCapUsername.Size = new System.Drawing.Size(59, 15);
            this.lblCapUsername.TabIndex = 1;
            this.lblCapUsername.Text = "Tài khoản";
            // 
            // valUsername
            // 
            this.valUsername.AutoSize = true;
            this.valUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.valUsername.Location = new System.Drawing.Point(20, 77);
            this.valUsername.Name = "valUsername";
            this.valUsername.Size = new System.Drawing.Size(51, 20);
            this.valUsername.TabIndex = 2;
            this.valUsername.Text = "admin";
            // 
            // lblCapFullName
            // 
            this.lblCapFullName.AutoSize = true;
            this.lblCapFullName.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblCapFullName.Location = new System.Drawing.Point(20, 114);
            this.lblCapFullName.Name = "lblCapFullName";
            this.lblCapFullName.Size = new System.Drawing.Size(60, 15);
            this.lblCapFullName.TabIndex = 3;
            this.lblCapFullName.Text = "Họ và tên";
            // 
            // valFullName
            // 
            this.valFullName.AutoSize = true;
            this.valFullName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.valFullName.Location = new System.Drawing.Point(20, 133);
            this.valFullName.Name = "valFullName";
            this.valFullName.Size = new System.Drawing.Size(65, 20);
            this.valFullName.TabIndex = 4;
            this.valFullName.Text = "Chủ Web";
            // 
            // lblCapRole
            // 
            this.lblCapRole.AutoSize = true;
            this.lblCapRole.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblCapRole.Location = new System.Drawing.Point(20, 170);
            this.lblCapRole.Name = "lblCapRole";
            this.lblCapRole.Size = new System.Drawing.Size(43, 15);
            this.lblCapRole.TabIndex = 5;
            this.lblCapRole.Text = "Vai trò";
            // 
            // valRole
            // 
            this.valRole.AutoSize = true;
            this.valRole.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.valRole.Location = new System.Drawing.Point(20, 189);
            this.valRole.Name = "valRole";
            this.valRole.Size = new System.Drawing.Size(53, 20);
            this.valRole.TabIndex = 6;
            this.valRole.Text = "Admin";
            // 
            // lblCapLoginTime
            // 
            this.lblCapLoginTime.AutoSize = true;
            this.lblCapLoginTime.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapLoginTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblCapLoginTime.Location = new System.Drawing.Point(20, 226);
            this.lblCapLoginTime.Name = "lblCapLoginTime";
            this.lblCapLoginTime.Size = new System.Drawing.Size(117, 15);
            this.lblCapLoginTime.TabIndex = 7;
            this.lblCapLoginTime.Text = "Thời gian đăng nhập";
            // 
            // valLoginTime
            // 
            this.valLoginTime.AutoSize = true;
            this.valLoginTime.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valLoginTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.valLoginTime.Location = new System.Drawing.Point(20, 245);
            this.valLoginTime.Name = "valLoginTime";
            this.valLoginTime.Size = new System.Drawing.Size(134, 19);
            this.valLoginTime.TabIndex = 8;
            this.valLoginTime.Text = "14:23:14 24/10/2026";
            // 
            // pnlCardService
            // 
            this.pnlCardService.BackColor = System.Drawing.Color.White;
            this.pnlCardService.Controls.Add(this.btnCheckConnection);
            this.pnlCardService.Controls.Add(this.lblCheckTimestamp);
            this.pnlCardService.Controls.Add(this.lblStatusIndicator);
            this.pnlCardService.Controls.Add(this.valApiUrl);
            this.pnlCardService.Controls.Add(this.lblCapApiUrl);
            this.pnlCardService.Controls.Add(this.lblServiceTitle);
            this.pnlCardService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardService.Location = new System.Drawing.Point(496, 0);
            this.pnlCardService.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.pnlCardService.Name = "pnlCardService";
            this.pnlCardService.Padding = new System.Windows.Forms.Padding(20);
            this.pnlCardService.Size = new System.Drawing.Size(496, 320);
            this.pnlCardService.TabIndex = 1;
            // 
            // lblServiceTitle
            // 
            this.lblServiceTitle.AutoSize = true;
            this.lblServiceTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            this.lblServiceTitle.Location = new System.Drawing.Point(18, 18);
            this.lblServiceTitle.Name = "lblServiceTitle";
            this.lblServiceTitle.Size = new System.Drawing.Size(142, 20);
            this.lblServiceTitle.TabIndex = 0;
            this.lblServiceTitle.Text = "KẾT NỐI DỊCH VỤ";
            // 
            // lblCapApiUrl
            // 
            this.lblCapApiUrl.AutoSize = true;
            this.lblCapApiUrl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapApiUrl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblCapApiUrl.Location = new System.Drawing.Point(20, 58);
            this.lblCapApiUrl.Name = "lblCapApiUrl";
            this.lblCapApiUrl.Size = new System.Drawing.Size(103, 15);
            this.lblCapApiUrl.TabIndex = 1;
            this.lblCapApiUrl.Text = "API đang sử dụng";
            // 
            // valApiUrl
            // 
            this.valApiUrl.AutoSize = true;
            this.valApiUrl.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valApiUrl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.valApiUrl.Location = new System.Drawing.Point(20, 77);
            this.valApiUrl.Name = "valApiUrl";
            this.valApiUrl.Size = new System.Drawing.Size(183, 19);
            this.valApiUrl.TabIndex = 2;
            this.valApiUrl.Text = "http://localhost:5000/api/";
            // 
            // lblStatusIndicator
            // 
            this.lblStatusIndicator.AutoSize = true;
            this.lblStatusIndicator.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusIndicator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.lblStatusIndicator.Location = new System.Drawing.Point(20, 126);
            this.lblStatusIndicator.Name = "lblStatusIndicator";
            this.lblStatusIndicator.Size = new System.Drawing.Size(189, 21);
            this.lblStatusIndicator.TabIndex = 3;
            this.lblStatusIndicator.Text = "● Kết nối API thành công";
            // 
            // lblCheckTimestamp
            // 
            this.lblCheckTimestamp.AutoSize = true;
            this.lblCheckTimestamp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckTimestamp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblCheckTimestamp.Location = new System.Drawing.Point(22, 154);
            this.lblCheckTimestamp.Name = "lblCheckTimestamp";
            this.lblCheckTimestamp.Size = new System.Drawing.Size(164, 15);
            this.lblCheckTimestamp.TabIndex = 4;
            this.lblCheckTimestamp.Text = "Kiểm tra lúc 14:23:14 24/10/2026";
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(107)))));
            this.btnCheckConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheckConnection.FlatAppearance.BorderSize = 0;
            this.btnCheckConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckConnection.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckConnection.ForeColor = System.Drawing.Color.White;
            this.btnCheckConnection.Location = new System.Drawing.Point(24, 190);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(160, 36);
            this.btnCheckConnection.TabIndex = 5;
            this.btnCheckConnection.Text = "Kiểm tra kết nối";
            this.btnCheckConnection.UseVisualStyleBackColor = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(246)))));
            this.pnlFooter.Controls.Add(this.lblFooterNote);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 565);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(24, 8, 24, 14);
            this.pnlFooter.Size = new System.Drawing.Size(1040, 55);
            this.pnlFooter.TabIndex = 2;
            // 
            // lblFooterNote
            // 
            this.lblFooterNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooterNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooterNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(144)))), ((int)(((byte)(156)))));
            this.lblFooterNote.Location = new System.Drawing.Point(24, 8);
            this.lblFooterNote.Name = "lblFooterNote";
            this.lblFooterNote.Size = new System.Drawing.Size(992, 33);
            this.lblFooterNote.TabIndex = 0;
            this.lblFooterNote.Text = "Lưu ý: các trường Hồ sơ cửa hàng và cấu hình POS có thể được bổ sung khi có bảng cấu hình tương ứng trong cơ sở dữ liệu. Trang này không lưu cài đặt giả vào bộ nhớ tạm.";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1040, 620);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlTopBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormSettings";
            this.Text = "Cài đặt hệ thống";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.tblCards.ResumeLayout(false);
            this.pnlCardSession.ResumeLayout(false);
            this.pnlCardSession.PerformLayout();
            this.pnlCardService.ResumeLayout(false);
            this.pnlCardService.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblHeaderMainTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblHeaderUserInfo;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tblCards;
        private System.Windows.Forms.Panel pnlCardSession;
        private System.Windows.Forms.Label lblSessionTitle;
        private System.Windows.Forms.Label lblCapUsername;
        private System.Windows.Forms.Label valUsername;
        private System.Windows.Forms.Label lblCapFullName;
        private System.Windows.Forms.Label valFullName;
        private System.Windows.Forms.Label lblCapRole;
        private System.Windows.Forms.Label valRole;
        private System.Windows.Forms.Label lblCapLoginTime;
        private System.Windows.Forms.Label valLoginTime;
        private System.Windows.Forms.Panel pnlCardService;
        private System.Windows.Forms.Label lblServiceTitle;
        private System.Windows.Forms.Label lblCapApiUrl;
        private System.Windows.Forms.Label valApiUrl;
        private System.Windows.Forms.Label lblStatusIndicator;
        private System.Windows.Forms.Label lblCheckTimestamp;
        private System.Windows.Forms.Button btnCheckConnection;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooterNote;
    }
}