using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormPOS : Form
    {
        // ===================== DỮ LIỆU =====================
        private readonly List<CartItemDto> _cart = new List<CartItemDto>();
        private readonly BindingSource _cartSource = new BindingSource();
        private List<ProductDto> _allProducts = new List<ProductDto>();
        private CustomerDto _selectedCustomer = null;
        private bool _formattingCash;

        // ===================== CONTROL =====================
        private DataGridView dgvCart = new DataGridView();
        private DataGridView dgvProducts = new DataGridView();
        private Label lblTotalAmount;
        private Label lblChange;
        private Label lblCustomerName;
        private Label lblCashier;
        private Label lblClock;
        private TextBox txtBarcode;
        private TextBox txtCashReceived;
        private TextBox txtCustomerPhone;
        private TextBox txtProductSearch;
        private Button btnCheckout;
        private Button btnPayCash;
        private Button btnPayTransfer;
        private string _paymentMethod = "Cash";
        private Button btnRemoveItem;
        private Button btnClearCart;
        private Button btnIncrease;
        private Button btnDecrease;
        private Button btnFindCustomer;
        private Button btnReloadProducts;
        private Timer _clockTimer;

        // ===================== MÀU SẮC =====================
        private static readonly Color PrimaryColor = Color.FromArgb(0, 121, 107);
        private static readonly Color DarkColor = Color.FromArgb(0, 77, 64);
        private static readonly Color BackColorMain = Color.FromArgb(240, 244, 243);
        private static readonly Color DangerColor = Color.FromArgb(211, 47, 47);
        private static readonly Color SuccessColor = Color.FromArgb(46, 125, 50);

        public FormPOS()
        {
            InitializeComponent();
            SetupCartGrid();
            SetupProductGrid();

            lblTotalAmount.Text = "0 đ";
            lblChange.Text = "0 đ";
            lblCustomerName.Text = "Khách vãng lai";
            lblCashier.Text = "Thu ngân: " + SessionManager.CurrentUsername;
        }

        // =====================================================
        // 0. DỰNG GIAO DIỆN
        // =====================================================
        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Bán hàng - POS";
            Size = new Size(1280, 760);
            MinimumSize = new Size(1000, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            BackColor = BackColorMain;
            KeyPreview = true;

            // ---------- HEADER ----------
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = PrimaryColor
            };

            var lblTitle = new Label
            {
                Text = "QUẦY BÁN HÀNG - THANH TOÁN (POS)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 10, 0, 0)
            };

            var headerRight = new Panel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                Padding = new Padding(0, 0, 16, 0)
            };

            lblClock = new Label
            {
                ForeColor = Color.FromArgb(200, 240, 235),
                Dock = DockStyle.Right,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Padding = new Padding(12, 11, 0, 0)
            };

            lblCashier = new Label
            {
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9.5F),
                Padding = new Padding(0, 11, 0, 0)
            };

            headerRight.Controls.Add(lblCashier);
            headerRight.Controls.Add(lblClock);
            header.Controls.Add(headerRight);
            header.Controls.Add(lblTitle);

            // ---------- BỐ CỤC CHÍNH ----------
            var main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(10),
                BackColor = BackColorMain
            };
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 390F));

            // ---------- CỘT TRÁI ----------
            var left = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Margin = new Padding(0, 0, 10, 0)
            };
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));

            // -- 1. Thanh quét mã & Thao tác giỏ --
            var barcodeBar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(0, 0, 0, 6)
            };

            var barcodeTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            barcodeTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            barcodeTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var barcodeInputPanel = new Panel { Dock = DockStyle.Fill };
            var lblBarcode = new Label
            {
                Text = "MÃ VẠCH (F2)",
                Dock = DockStyle.Top,
                Height = 18,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };
            txtBarcode = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 13F),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtBarcode.KeyDown += txtBarcode_KeyDown;
            barcodeInputPanel.Controls.Add(txtBarcode);
            barcodeInputPanel.Controls.Add(lblBarcode);

            var cartActionFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(10, 10, 0, 0)
            };

            btnIncrease = MakeButton("+", PrimaryColor, new Font("Segoe UI", 12F, FontStyle.Bold));
            btnIncrease.Size = new Size(38, 32);
            btnIncrease.Margin = new Padding(2, 0, 2, 0);

            btnDecrease = MakeButton("-", PrimaryColor, new Font("Segoe UI", 12F, FontStyle.Bold));
            btnDecrease.Size = new Size(38, 32);
            btnDecrease.Margin = new Padding(2, 0, 4, 0);

            btnRemoveItem = MakeButton("Xóa dòng", DangerColor, new Font("Segoe UI", 9.5F, FontStyle.Bold));
            btnRemoveItem.Size = new Size(95, 32);
            btnRemoveItem.Margin = new Padding(2, 0, 4, 0);

            btnClearCart = MakeButton("Làm trống", Color.FromArgb(120, 130, 140), new Font("Segoe UI", 9.5F, FontStyle.Bold));
            btnClearCart.Size = new Size(95, 32);
            btnClearCart.Margin = new Padding(2, 0, 0, 0);

            btnIncrease.Click += btnIncrease_Click;
            btnDecrease.Click += btnDecrease_Click;
            btnRemoveItem.Click += btnRemoveItem_Click;
            btnClearCart.Click += btnClearCart_Click;

            cartActionFlow.Controls.AddRange(new Control[]
            {
                btnIncrease, btnDecrease, btnRemoveItem, btnClearCart
            });

            barcodeTable.Controls.Add(barcodeInputPanel, 0, 0);
            barcodeTable.Controls.Add(cartActionFlow, 1, 0);
            barcodeBar.Controls.Add(barcodeTable);

            // -- 2. Giỏ hàng --
            StyleGrid(dgvCart);
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.Margin = new Padding(0, 0, 0, 6);

            // -- 3. Thanh tìm kiếm --
            var searchBar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(0, 0, 0, 6)
            };

            var searchTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            searchTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F));
            searchTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            searchTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            searchTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var lblSearch = new Label
            {
                Text = "DANH MỤC HÀNG HÓA",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = DarkColor,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            txtProductSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtProductSearch.TextChanged += txtProductSearch_TextChanged;

            btnReloadProducts = MakeButton("Tải lại", Color.FromArgb(84, 110, 122), new Font("Segoe UI", 9.5F, FontStyle.Bold));
            btnReloadProducts.Size = new Size(85, 30);
            btnReloadProducts.Margin = new Padding(6, 1, 6, 0);
            btnReloadProducts.Click += async (s, e) => await LoadProductCatalogAsync();

            var lblHint = new Label
            {
                Text = "(Nhấp đúp chuột để chọn)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                Padding = new Padding(4, 0, 0, 0)
            };

            searchTable.Controls.Add(lblSearch, 0, 0);
            searchTable.Controls.Add(txtProductSearch, 1, 0);
            searchTable.Controls.Add(btnReloadProducts, 2, 0);
            searchTable.Controls.Add(lblHint, 3, 0);
            searchBar.Controls.Add(searchTable);

            // -- 4. Danh mục sản phẩm --
            StyleGrid(dgvProducts);
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.Margin = new Padding(0);
            dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;

            left.Controls.Add(barcodeBar, 0, 0);
            left.Controls.Add(dgvCart, 0, 1);
            left.Controls.Add(searchBar, 0, 2);
            left.Controls.Add(dgvProducts, 0, 3);

            // ---------- CỘT PHẢI (THANH TOÁN) ----------
            var right = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };

            var rightTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 11,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 134F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            rightTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));

            // 1. Khách hàng
            var pnlCustomer = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            var lblCustHeader = MakeCaptionLabel("KHÁCH HÀNG (TÊN / SĐT)");

            var pnlCustInput = new Panel { Dock = DockStyle.Top, Height = 32 };
            btnFindCustomer = MakeButton("Tìm", PrimaryColor, new Font("Segoe UI", 9.5F, FontStyle.Bold));
            btnFindCustomer.Dock = DockStyle.Right;
            btnFindCustomer.Width = 75;
            btnFindCustomer.Click += async (s, e) => await FindCustomerAsync();

            var spacerCust = new Panel { Dock = DockStyle.Right, Width = 6 };

            txtCustomerPhone = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtCustomerPhone.KeyDown += txtCustomerPhone_KeyDown;

            pnlCustInput.Controls.Add(txtCustomerPhone);
            pnlCustInput.Controls.Add(spacerCust);
            pnlCustInput.Controls.Add(btnFindCustomer);

            lblCustomerName = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(55, 71, 79),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Khách vãng lai"
            };

            pnlCustomer.Controls.Add(lblCustomerName);
            pnlCustomer.Controls.Add(pnlCustInput);
            pnlCustomer.Controls.Add(lblCustHeader);

            // 2. Tổng tiền
            var pnlTotal = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            var lblTotalHeader = MakeCaptionLabel("TỔNG TIỀN THANH TOÁN");
            lblTotalAmount = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                ForeColor = DangerColor,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.FromArgb(255, 243, 243),
                Padding = new Padding(0, 0, 10, 0)
            };
            pnlTotal.Controls.Add(lblTotalAmount);
            pnlTotal.Controls.Add(lblTotalHeader);

            // 3. Tiền khách đưa
            var pnlCash = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            var lblCashHeader = MakeCaptionLabel("TIỀN KHÁCH ĐƯA (F4)");
            txtCashReceived = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtCashReceived.TextChanged += txtCashReceived_TextChanged;

            var pnlQuickCash = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                ColumnCount = 4,
                RowCount = 1,
                Margin = new Padding(0, 5, 0, 5)
            };
            pnlQuickCash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            pnlQuickCash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            pnlQuickCash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            pnlQuickCash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            int[] denominations = { 50000, 100000, 200000, 500000 };
            for (int i = 0; i < denominations.Length; i++)
            {
                int val = denominations[i];
                var btnDenom = MakeButton((val / 1000) + "k", Color.FromArgb(0, 150, 136), new Font("Segoe UI", 9.5F, FontStyle.Bold));
                btnDenom.Dock = DockStyle.Fill;
                btnDenom.Margin = new Padding(i == 0 ? 0 : 2, 0, i == 3 ? 0 : 2, 0);
                btnDenom.Click += (s, e) => txtCashReceived.Text = val.ToString();
                pnlQuickCash.Controls.Add(btnDenom, i, 0);
            }

            var btnExact = MakeButton("Đủ tiền", Color.FromArgb(245, 124, 0), new Font("Segoe UI", 9.5F, FontStyle.Bold));
            btnExact.Dock = DockStyle.Top;
            btnExact.Height = 30;
            btnExact.Margin = new Padding(0, 4, 0, 0);
            btnExact.Click += (s, e) =>
            {
                decimal total = _cart.Sum(item => item.TotalPrice);
                txtCashReceived.Text = ((long)total).ToString();
            };

            pnlCash.Controls.Add(btnExact);
            pnlCash.Controls.Add(pnlQuickCash);
            pnlCash.Controls.Add(txtCashReceived);
            pnlCash.Controls.Add(lblCashHeader);

            // 4. Tiền thừa
            var pnlChange = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            var lblChangeHeader = MakeCaptionLabel("TIỀN THỪA TRẢ KHÁCH");
            lblChange = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = SuccessColor,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.FromArgb(232, 245, 233),
                Padding = new Padding(0, 0, 10, 0)
            };
            pnlChange.Controls.Add(lblChange);
            pnlChange.Controls.Add(lblChangeHeader);

            // 5. Phương thức thanh toán
            var pnlPayMethods = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            pnlPayMethods.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlPayMethods.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnPayCash = MakeButton("Tiền mặt", PrimaryColor, new Font("Segoe UI", 10.5F, FontStyle.Bold));
            btnPayCash.Dock = DockStyle.Fill;
            btnPayCash.Margin = new Padding(0, 0, 4, 0);
            btnPayCash.Cursor = Cursors.Hand;
            btnPayCash.Click += (s, e) => SetPaymentMethod("Cash");

            btnPayTransfer = MakeButton("Chuyển khoản", Color.FromArgb(240, 244, 244), new Font("Segoe UI", 10.5F, FontStyle.Bold));
            btnPayTransfer.ForeColor = Color.FromArgb(70, 80, 90);
            btnPayTransfer.Dock = DockStyle.Fill;
            btnPayTransfer.Margin = new Padding(4, 0, 0, 0);
            btnPayTransfer.Cursor = Cursors.Hand;
            btnPayTransfer.Click += (s, e) => SetPaymentMethod("Transfer");

            pnlPayMethods.Controls.Add(btnPayCash, 0, 0);
            pnlPayMethods.Controls.Add(btnPayTransfer, 1, 0);

            // 6. Nút thanh toán
            btnCheckout = MakeButton("THANH TOÁN (F9)", SuccessColor, new Font("Segoe UI", 15F, FontStyle.Bold));
            btnCheckout.Dock = DockStyle.Fill;
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Click += btnCheckout_Click;

            rightTable.Controls.Add(pnlCustomer, 0, 0);
            rightTable.Controls.Add(pnlTotal, 0, 2);
            rightTable.Controls.Add(pnlCash, 0, 4);
            rightTable.Controls.Add(pnlChange, 0, 6);
            rightTable.Controls.Add(pnlPayMethods, 0, 8);
            rightTable.Controls.Add(btnCheckout, 0, 10);

            right.Controls.Add(rightTable);

            main.Controls.Add(left, 0, 0);
            main.Controls.Add(right, 1, 0);

            Controls.Add(header);
            Controls.Add(main);
            header.SendToBack();

            Load += FormPOS_Load;
            FormClosed += (s, e) => _clockTimer?.Stop();

            ResumeLayout(false);
        }

        private void SetPaymentMethod(string method)
        {
            _paymentMethod = method;
            if (method == "Transfer")
            {
                btnPayTransfer.BackColor = Color.FromArgb(2, 136, 209);
                btnPayTransfer.ForeColor = Color.White;
                btnPayCash.BackColor = Color.FromArgb(240, 244, 244);
                btnPayCash.ForeColor = Color.FromArgb(70, 80, 90);
                decimal total = _cart.Sum(item => item.TotalPrice);
                txtCashReceived.Text = ((long)total).ToString();
            }
            else
            {
                btnPayCash.BackColor = PrimaryColor;
                btnPayCash.ForeColor = Color.White;
                btnPayTransfer.BackColor = Color.FromArgb(240, 244, 244);
                btnPayTransfer.ForeColor = Color.FromArgb(70, 80, 90);
                decimal total = _cart.Sum(item => item.TotalPrice);
                if (string.IsNullOrWhiteSpace(txtCashReceived.Text) || txtCashReceived.Text == "0")
                {
                    txtCashReceived.Text = ((long)total).ToString();
                }
            }
            CalculateChange();
        }

        private static Button MakeButton(string text, Color color, Font font = null)
        {
            var b = new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = font ?? new Font("Segoe UI", 10F, FontStyle.Bold),
                TabStop = false
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private static Label MakeCaptionLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 20,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };
        }

        private static void StyleGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = DarkColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 34;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.RowTemplate.Height = 32;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 250, 249);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(178, 223, 219);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.GridColor = Color.FromArgb(224, 224, 224);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.AllowUserToResizeRows = false;
        }

        // =====================================================
        // 1. SỰ KIỆN FORM / PHÍM TẮT
        // =====================================================
        private async void FormPOS_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();

            lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
            _clockTimer = new Timer { Interval = 1000 };
            _clockTimer.Tick += (s, ev) =>
                lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
            _clockTimer.Start();

            await LoadProductCatalogAsync();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    txtBarcode.Focus();
                    return true;
                case Keys.F4:
                    txtCashReceived.Focus();
                    return true;
                case Keys.F7:
                    SetPaymentMethod("Cash");
                    return true;
                case Keys.F8:
                    SetPaymentMethod("Transfer");
                    return true;
                case Keys.F9:
                    btnCheckout.PerformClick();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // =====================================================
        // 2. CẤU HÌNH BẢNG
        // =====================================================
        private void SetupCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.AllowUserToAddRows = false;
            dgvCart.ReadOnly = true;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.MultiSelect = false;
            dgvCart.Columns.Clear();

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProductId",
                DataPropertyName = nameof(CartItemDto.ProductId),
                HeaderText = "Mã SP",
                Width = 70,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colProductName",
                DataPropertyName = nameof(CartItemDto.ProductName),
                HeaderText = "Tên sản phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colUnitPrice",
                DataPropertyName = nameof(CartItemDto.UnitPrice),
                HeaderText = "Đơn giá",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colQuantity",
                DataPropertyName = nameof(CartItemDto.Quantity),
                HeaderText = "SL",
                Width = 65,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold) }
            });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTotalPrice",
                DataPropertyName = nameof(CartItemDto.TotalPrice),
                HeaderText = "Thành tiền",
                Width = 125,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 10F, FontStyle.Bold) }
            });

            _cartSource.DataSource = _cart;
            dgvCart.DataSource = _cartSource;
        }

        private void SetupProductGrid()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.Columns.Clear();

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductDto.Barcode),
                HeaderText = "Mã vạch",
                Width = 130
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductDto.ProductName),
                HeaderText = "Tên sản phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductDto.Price),
                HeaderText = "Giá bán",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductDto.StockQuantity),
                HeaderText = "Tồn kho",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvProducts.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (dgvProducts.Rows[e.RowIndex].DataBoundItem is ProductDto p && p.StockQuantity <= 0)
                    e.CellStyle.ForeColor = DangerColor;
            };
        }

        // =====================================================
        // 3. LẤY DỮ LIỆU TỪ DB (QUA API)
        // =====================================================
        private async Task LoadProductCatalogAsync()
        {
            try
            {
                btnReloadProducts.Enabled = false;
                var products = await ApiClientService.Client.GetFromJsonAsync<List<ProductDto>>("products");
                _allProducts = products ?? new List<ProductDto>();
                ApplyProductFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh mục sản phẩm từ API.\n\n" + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnReloadProducts.Enabled = true;
            }
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e) => ApplyProductFilter();

        private void ApplyProductFilter()
        {
            string keyword = (txtProductSearch.Text ?? string.Empty).Trim();
            IEnumerable<ProductDto> query = _allProducts;
            if (keyword.Length > 0)
            {
                query = query.Where(p =>
                    (p.ProductName ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (p.Barcode ?? "").IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            dgvProducts.DataSource = query.ToList();
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvProducts.Rows[e.RowIndex].DataBoundItem is ProductDto product)
            {
                AddProductToCart(product);
                txtBarcode.Focus();
            }
        }

        private async void txtCustomerPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            await FindCustomerAsync();
        }

        private async Task FindCustomerAsync()
        {
            string keyword = txtCustomerPhone.Text.Trim();
            if (keyword.Length == 0)
            {
                _selectedCustomer = null;
                lblCustomerName.Text = "Khách vãng lai";
                lblCustomerName.ForeColor = Color.FromArgb(55, 71, 79);
                return;
            }

            try
            {
                CustomerDto matchedCustomer = null;
                try
                {
                    var searchResults = await ApiClientService.Client
                        .GetFromJsonAsync<List<CustomerDto>>($"customers/search?keyword={Uri.EscapeDataString(keyword)}");

                    if (searchResults != null && searchResults.Count > 0)
                    {
                        matchedCustomer = searchResults.FirstOrDefault(c =>
                            string.Equals(c.PhoneNumber, keyword, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(c.FullName, keyword, StringComparison.OrdinalIgnoreCase))
                            ?? searchResults[0];
                    }
                }
                catch { }

                if (matchedCustomer == null)
                {
                    HttpResponseMessage response = await ApiClientService.Client
                        .GetAsync($"customers/phone/{Uri.EscapeDataString(keyword)}");

                    if (response.IsSuccessStatusCode)
                    {
                        matchedCustomer = await response.Content.ReadFromJsonAsync<CustomerDto>();
                    }
                }

                if (matchedCustomer != null)
                {
                    _selectedCustomer = matchedCustomer;
                    string rank = string.IsNullOrEmpty(matchedCustomer.MembershipRank) ? "Thành viên" : matchedCustomer.MembershipRank;
                    lblCustomerName.Text = $"{matchedCustomer.FullName} ({rank})  •  {matchedCustomer.Points:N0} điểm";
                    lblCustomerName.ForeColor = Color.FromArgb(0, 121, 107);
                }
                else
                {
                    _selectedCustomer = null;
                    lblCustomerName.Text = "Chưa có trong hệ thống (khách mới)";
                    lblCustomerName.ForeColor = Color.Firebrick;
                }
            }
            catch (Exception ex)
            {
                _selectedCustomer = null;
                lblCustomerName.Text = "Lỗi kết nối: " + ex.Message;
                lblCustomerName.ForeColor = Color.Firebrick;
            }
        }

        // =====================================================
        // 4. QUÉT MÃ VẠCH & XỬ LÝ GIỎ HÀNG
        // =====================================================
        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            string barcode = txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Vui lòng nhập hoặc quét mã vạch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarcode.Focus();
                return;
            }

            txtBarcode.Clear();
            await AddProductToCartByBarcodeAsync(barcode);
            txtBarcode.Focus();
        }

        private async Task AddProductToCartByBarcodeAsync(string barcode)
        {
            try
            {
                string encodedBarcode = Uri.EscapeDataString(barcode);
                ProductDto product = await ApiClientService.Client
                    .GetFromJsonAsync<ProductDto>($"products/barcode/{encodedBarcode}");

                if (product == null)
                {
                    MessageBox.Show($"Không tìm thấy sản phẩm có mã vạch: {barcode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AddProductToCart(product);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm sản phẩm qua mã vạch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddProductToCart(ProductDto product)
        {
            if (product.StockQuantity <= 0)
            {
                MessageBox.Show($"Sản phẩm [{product.ProductName}] đã hết hàng trong kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingItem = _cart.FirstOrDefault(i => i.ProductId == product.ProductId);
            if (existingItem != null)
            {
                if (existingItem.Quantity + 1 > product.StockQuantity)
                {
                    MessageBox.Show("Số lượng trong giỏ vượt quá tồn kho hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existingItem.Quantity++;
            }
            else
            {
                _cart.Add(new CartItemDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.Price,
                    Quantity = 1,
                    StockQuantity = product.StockQuantity
                });
            }

            UpdateCartDisplay();
        }

        private void UpdateCartDisplay()
        {
            _cartSource.ResetBindings(false);

            decimal totalAmount = _cart.Sum(i => i.TotalPrice);
            lblTotalAmount.Text = totalAmount.ToString("N0") + " đ";

            if (_paymentMethod == "Transfer")
            {
                txtCashReceived.Text = ((long)totalAmount).ToString();
            }

            CalculateChange();
        }

        private void CalculateChange()
        {
            decimal totalAmount = _cart.Sum(i => i.TotalPrice);
            decimal cashReceived = 0;
            decimal.TryParse(txtCashReceived.Text.Replace(",", "").Replace("đ", "").Trim(), out cashReceived);

            decimal change = cashReceived - totalAmount;
            lblChange.Text = (change >= 0 ? change : 0).ToString("N0") + " đ";
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            if (_formattingCash) return;
            _formattingCash = true;

            try
            {
                string raw = txtCashReceived.Text.Replace(",", "").Replace(".", "").Trim();
                if (long.TryParse(raw, out long val))
                {
                    txtCashReceived.Text = val.ToString("N0", CultureInfo.InvariantCulture);
                    txtCashReceived.SelectionStart = txtCashReceived.Text.Length;
                }
            }
            catch { }
            finally
            {
                _formattingCash = false;
                CalculateChange();
            }
        }

        // =====================================================
        // 5. CÁC NÚT THAO TÁC TRÊN GIỎ HÀNG
        // =====================================================
        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is CartItemDto item)
            {
                if (item.Quantity + 1 > item.StockQuantity)
                {
                    MessageBox.Show("Đã đạt giới hạn tồn kho của sản phẩm này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                item.Quantity++;
                UpdateCartDisplay();
            }
        }

        private void btnDecrease_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is CartItemDto item)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    _cart.Remove(item);
                }
                UpdateCartDisplay();
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow?.DataBoundItem is CartItemDto item)
            {
                _cart.Remove(item);
                UpdateCartDisplay();
            }
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0) return;
            if (MessageBox.Show("Bạn có chắc chắn muốn làm trống giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _cart.Clear();
                UpdateCartDisplay();
            }
        }

        // =====================================================
        // 6. THANH TOÁN (CHECKOUT)
        // =====================================================
        private async void btnCheckout_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalAmount = _cart.Sum(i => i.TotalPrice);
            decimal cashReceived = 0;
            decimal.TryParse(txtCashReceived.Text.Replace(",", "").Replace("đ", "").Trim(), out cashReceived);

            if (_paymentMethod == "Transfer")
            {
                cashReceived = totalAmount;
            }
            else if (_paymentMethod == "Cash" && cashReceived < totalAmount)
            {
                MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán đơn hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCashReceived.Focus();
                return;
            }

            decimal change = cashReceived >= totalAmount ? cashReceived - totalAmount : 0;

            try
            {
                btnCheckout.Enabled = false;

                // Tự động tìm khách hàng nếu thu ngân có gõ SĐT/tên nhưng chưa ấn Enter
                if (_selectedCustomer == null && !string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
                {
                    await FindCustomerAsync();
                }

                var orderDto = new
                {
                    CustomerId = _selectedCustomer?.CustomerId,
                    CashierUsername = SessionManager.CurrentUsername,
                    CustomerPhone = _selectedCustomer?.PhoneNumber ?? txtCustomerPhone.Text.Trim(),
                    PaymentMethod = _paymentMethod,
                    TotalAmount = totalAmount,
                    CashReceived = cashReceived,
                    ChangeAmount = change,
                    Items = _cart.Select(i => new {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList(),
                    Details = _cart.Select(i => new {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                HttpResponseMessage response = await ApiClientService.Client.PostAsJsonAsync("orders/checkout", orderDto);
                if (!response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    response = await ApiClientService.Client.PostAsJsonAsync("orders", orderDto);
                }

                if (response.IsSuccessStatusCode)
                {
                    string methodText = _paymentMethod == "Transfer" ? "Chuyển khoản" : "Tiền mặt";
                    MessageBox.Show(
                        $"Thanh toán thành công [{methodText}]!\nTiền thừa trả khách: {change:N0} đ",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _cart.Clear();
                    SetPaymentMethod("Cash");
                    UpdateCartDisplay();

                    txtCashReceived.Clear();
                    txtCustomerPhone.Clear();
                    _selectedCustomer = null;
                    lblCustomerName.Text = "Khách vãng lai";
                    lblCustomerName.ForeColor = Color.FromArgb(55, 71, 79);
                    txtBarcode.Focus();

                    await LoadProductCatalogAsync();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"Thanh toán thất bại!\nMã trạng thái: {(int)response.StatusCode}\nChi tiết: {errorContent}",
                        "Lỗi thanh toán",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi kết nối máy chủ thanh toán.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCheckout.Enabled = true;
            }
        }
    }

    // =========================================================
    // DTO SẢN PHẨM API TRẢ VỀ
    // =========================================================
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // =========================================================
    // DTO KHÁCH HÀNG API TRẢ VỀ
    // =========================================================
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Points { get; set; }

        public string PhoneNumber
        {
            get => Phone;
            set => Phone = value ?? string.Empty;
        }

        public int? RewardPoints
        {
            get => Points;
            set => Points = value ?? 0;
        }

        public string MembershipRank { get; set; } = string.Empty;
    }

    // =========================================================
    // DTO MẶT HÀNG TRONG GIỎ
    // =========================================================
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}