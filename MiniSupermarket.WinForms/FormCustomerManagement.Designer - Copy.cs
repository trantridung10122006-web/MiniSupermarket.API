namespace MiniSupermarket.WinForms
{
    partial class FormCustomerManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.tblStats = new System.Windows.Forms.TableLayoutPanel();
            this.pnlStatOrders = new System.Windows.Forms.Panel();
            this.lblStatOrdersValue = new System.Windows.Forms.Label();
            this.lblStatOrdersCaption = new System.Windows.Forms.Label();
            this.pnlStatSpent = new System.Windows.Forms.Panel();
            this.lblStatSpentValue = new System.Windows.Forms.Label();
            this.lblStatSpentCaption = new System.Windows.Forms.Label();
            this.pnlStatPoints = new System.Windows.Forms.Panel();
            this.lblStatPointsValue = new System.Windows.Forms.Label();
            this.lblStatPointsCaption = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.btnOpenEdit = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.tblStats.SuspendLayout();
            this.pnlStatOrders.SuspendLayout();
            this.pnlStatSpent.SuspendLayout();
            this.pnlStatPoints.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(0, 121, 107);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1280, 56);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblTitle.Text = "🧾  THEO DÕI KHÁCH HÀNG & LỊCH SỬ MUA HÀNG";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.dgvCustomers);
            this.pnlLeft.Controls.Add(this.pnlSearch);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(12, 12, 6, 12);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnReload);
            this.pnlSearch.Controls.Add(this.lblCount);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(600, 48);
            this.pnlSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.Gray;
            this.lblSearch.Location = new System.Drawing.Point(12, 15);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Text = "TÌM KIẾM";
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(90, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(230, 27);
            this.txtSearch.TabIndex = 0;
            // 
            // btnReload
            // 
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(96, 125, 139);
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Location = new System.Drawing.Point(332, 9);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(80, 30);
            this.btnReload.TabIndex = 1;
            this.btnReload.TabStop = false;
            this.btnReload.Text = "Tải lại";
            this.btnReload.UseVisualStyleBackColor = false;
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(0, 121, 107);
            this.lblCount.Location = new System.Drawing.Point(440, 15);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(150, 20);
            this.lblCount.Text = "0 khách hàng";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.dgvOrders);
            this.pnlRight.Controls.Add(this.lblHistoryTitle);
            this.pnlRight.Controls.Add(this.tblStats);
            this.pnlRight.Controls.Add(this.pnlInfo);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(6, 12, 12, 12);
            this.pnlRight.Size = new System.Drawing.Size(640, 644);
            this.pnlRight.TabIndex = 1;
            // 
            // dgvOrders
            // 
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.TabIndex = 0;
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.ForeColor = System.Drawing.Color.FromArgb(0, 121, 107);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(622, 36);
            this.lblHistoryTitle.Text = "LỊCH SỬ MUA HÀNG";
            this.lblHistoryTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tblStats
            // 
            this.tblStats.ColumnCount = 3;
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tblStats.Controls.Add(this.pnlStatOrders, 0, 0);
            this.tblStats.Controls.Add(this.pnlStatSpent, 1, 0);
            this.tblStats.Controls.Add(this.pnlStatPoints, 2, 0);
            this.tblStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblStats.Name = "tblStats";
            this.tblStats.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tblStats.RowCount = 1;
            this.tblStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblStats.Size = new System.Drawing.Size(622, 92);
            this.tblStats.TabIndex = 2;
            // 
            // pnlStatOrders
            // 
            this.pnlStatOrders.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
            this.pnlStatOrders.Controls.Add(this.lblStatOrdersValue);
            this.pnlStatOrders.Controls.Add(this.lblStatOrdersCaption);
            this.pnlStatOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatOrders.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.pnlStatOrders.Name = "pnlStatOrders";
            // 
            // lblStatOrdersValue
            // 
            this.lblStatOrdersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatOrdersValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblStatOrdersValue.ForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.lblStatOrdersValue.Name = "lblStatOrdersValue";
            this.lblStatOrdersValue.Text = "0";
            this.lblStatOrdersValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatOrdersCaption
            // 
            this.lblStatOrdersCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatOrdersCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatOrdersCaption.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);
            this.lblStatOrdersCaption.Name = "lblStatOrdersCaption";
            this.lblStatOrdersCaption.Size = new System.Drawing.Size(100, 24);
            this.lblStatOrdersCaption.Text = "SỐ ĐƠN HÀNG";
            this.lblStatOrdersCaption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnlStatSpent
            // 
            this.pnlStatSpent.BackColor = System.Drawing.Color.FromArgb(232, 245, 233);
            this.pnlStatSpent.Controls.Add(this.lblStatSpentValue);
            this.pnlStatSpent.Controls.Add(this.lblStatSpentCaption);
            this.pnlStatSpent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatSpent.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.pnlStatSpent.Name = "pnlStatSpent";
            // 
            // lblStatSpentValue
            // 
            this.lblStatSpentValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatSpentValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblStatSpentValue.ForeColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.lblStatSpentValue.Name = "lblStatSpentValue";
            this.lblStatSpentValue.Text = "0 đ";
            this.lblStatSpentValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatSpentCaption
            // 
            this.lblStatSpentCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatSpentCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatSpentCaption.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);
            this.lblStatSpentCaption.Name = "lblStatSpentCaption";
            this.lblStatSpentCaption.Size = new System.Drawing.Size(100, 24);
            this.lblStatSpentCaption.Text = "TỔNG CHI TIÊU";
            this.lblStatSpentCaption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnlStatPoints
            // 
            this.pnlStatPoints.BackColor = System.Drawing.Color.FromArgb(255, 243, 224);
            this.pnlStatPoints.Controls.Add(this.lblStatPointsValue);
            this.pnlStatPoints.Controls.Add(this.lblStatPointsCaption);
            this.pnlStatPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatPoints.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.pnlStatPoints.Name = "pnlStatPoints";
            // 
            // lblStatPointsValue
            // 
            this.lblStatPointsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatPointsValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblStatPointsValue.ForeColor = System.Drawing.Color.FromArgb(239, 108, 0);
            this.lblStatPointsValue.Name = "lblStatPointsValue";
            this.lblStatPointsValue.Text = "0";
            this.lblStatPointsValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatPointsCaption
            // 
            this.lblStatPointsCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatPointsCaption.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatPointsCaption.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);
            this.lblStatPointsCaption.Name = "lblStatPointsCaption";
            this.lblStatPointsCaption.Size = new System.Drawing.Size(100, 24);
            this.lblStatPointsCaption.Text = "ĐIỂM TÍCH LŨY";
            this.lblStatPointsCaption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.White;
            this.pnlInfo.Controls.Add(this.lblName);
            this.pnlInfo.Controls.Add(this.lblPhone);
            this.pnlInfo.Controls.Add(this.lblEmail);
            this.pnlInfo.Controls.Add(this.lblAddress);
            this.pnlInfo.Controls.Add(this.btnOpenEdit);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(622, 150);
            this.pnlInfo.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(0, 77, 64);
            this.lblName.Location = new System.Drawing.Point(16, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(430, 36);
            this.lblName.Text = "Chọn một khách hàng";
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblPhone.Location = new System.Drawing.Point(16, 58);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(580, 24);
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblEmail.Location = new System.Drawing.Point(16, 86);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(580, 24);
            // 
            // lblAddress
            // 
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblAddress.Location = new System.Drawing.Point(16, 114);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(580, 24);
            // 
            // btnOpenEdit
            // 
            this.btnOpenEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenEdit.BackColor = System.Drawing.Color.FromArgb(25, 118, 210);
            this.btnOpenEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenEdit.FlatAppearance.BorderSize = 0;
            this.btnOpenEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenEdit.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnOpenEdit.ForeColor = System.Drawing.Color.White;
            this.btnOpenEdit.Location = new System.Drawing.Point(462, 14);
            this.btnOpenEdit.Name = "btnOpenEdit";
            this.btnOpenEdit.Size = new System.Drawing.Size(144, 34);
            this.btnOpenEdit.TabIndex = 0;
            this.btnOpenEdit.TabStop = false;
            this.btnOpenEdit.Text = "✎ Thêm / Sửa / Xóa";
            this.btnOpenEdit.UseVisualStyleBackColor = false;
            // 
            // FormCustomerManagement
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 244, 243);
            this.ClientSize = new System.Drawing.Size(1280, 700);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MinimumSize = new System.Drawing.Size(1150, 650);
            this.Name = "FormCustomerManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Theo dõi khách hàng";

            this.pnlHeader.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.pnlStatOrders.ResumeLayout(false);
            this.pnlStatSpent.ResumeLayout(false);
            this.pnlStatPoints.ResumeLayout(false);
            this.tblStats.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.TableLayoutPanel tblStats;
        private System.Windows.Forms.Panel pnlStatOrders;
        private System.Windows.Forms.Label lblStatOrdersValue;
        private System.Windows.Forms.Label lblStatOrdersCaption;
        private System.Windows.Forms.Panel pnlStatSpent;
        private System.Windows.Forms.Label lblStatSpentValue;
        private System.Windows.Forms.Label lblStatSpentCaption;
        private System.Windows.Forms.Panel pnlStatPoints;
        private System.Windows.Forms.Label lblStatPointsValue;
        private System.Windows.Forms.Label lblStatPointsCaption;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Button btnOpenEdit;
    }
}