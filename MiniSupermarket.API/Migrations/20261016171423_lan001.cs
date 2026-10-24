using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class lan001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RewardPoints = table.Column<int>(type: "int", nullable: true),
                    MembershipRank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId");
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandId", "BrandName", "Description" },
                values: new object[,]
                {
                    { 1, "Chinsu / Omachi", "Thương hiệu gia vị và thực phẩm ăn liền nổi tiếng" },
                    { 2, "Dutch Lady / Cô Gái Hà Lan", "Các sản phẩm sữa tươi và sữa chua dinh dưỡng" },
                    { 3, "Hoàng Dung Selected", "Thực phẩm khô và nông sản đóng gói nội bộ bách hóa" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Bánh kẹo & Đồ ăn vặt", "Snack, bánh quy, kẹo dẻo, sô-cô-la" },
                    { 2, "Nước giải khát & Trà", "Nước ngọt, nước khoáng, trà đóng chai, nước ép" },
                    { 3, "Sữa & Sản phẩm từ sữa", "Sữa tươi, sữa đặc, sữa chua, phô mai" },
                    { 4, "Mì gói & Thực phẩm ăn liền", "Mì ăn liền, phở khô, hủ tiếu, cháo gói" },
                    { 5, "Gia vị & Dầu ăn", "Nước mắm, hạt nêm, dầu ăn, xì dầu, bột ngọt" },
                    { 6, "Gạo & Nông sản khô", "Gạo ST25, đậu xanh, đậu đen, mộc nhĩ, nấm hương" },
                    { 7, "Đồ đóng hộp & Chế biến sẵn", "Cá hộp, thịt xông khói, xúc xích, pate" },
                    { 8, "Hóa mỹ phẩm & Giặt xả", "Nước giặt, nước xả vải, nước rửa chén, lau sàn" },
                    { 9, "Chăm sóc cá nhân", "Dầu gội, sữa tắm, kem đánh răng, xà phòng" },
                    { 10, "Thực phẩm đông lạnh", "Cá viên, bò viên, xúc xích đức, chả giò, bánh xếp" },
                    { 11, "Kem & Đồ lạnh", "Kem que, kem hộp, đá viên đóng túi" },
                    { 12, "Đồ dùng gia đình gia dụng", "Màng bọc thực phẩm, túi bóng, khăn giấy, mút rửa chén" },
                    { 13, "Bia & Đồ uống có cồn", "Bia lon, rượu nhẹ, nước trái cây lên men" },
                    { 14, "Rau củ quả tươi (Hàng ngày)", "Rau xanh, củ quả nguyên liệu nấu ăn hằng ngày" },
                    { 15, "Văn phòng phẩm & Đồ chơi nhỏ", "Bút, tập học sinh, đồ chơi trẻ em cơ bản" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "CreatedAt", "Email", "FullName", "MembershipRank", "PhoneNumber", "RewardPoints" },
                values: new object[,]
                {
                    { 1, "123 Lê Lợi, Q.1, TP.HCM", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nam.le@gmail.com", "Lê Hoàng Nam", "Bạc", "0987654321", 150 },
                    { 2, "456 Nguyễn Thị Minh Khai, Q.3, TP.HCM", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "lan.pham@gmail.com", "Phạm Thị Lan", "Vàng", "0978123456", 520 },
                    { 3, "789 Điện Biên Phủ, Q.Bình Thạnh, TP.HCM", new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "minh.tran@gmail.com", "Trần Văn Minh", "Kim Cương", "0912345678", 1200 },
                    { 4, "12 Trần Hưng Đạo, Q.5, TP.HCM", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "thanh.le@gmail.com", "Lê Thị Thanh", "Đồng", "0903112233", 45 },
                    { 5, "88 Nguyễn Trãi, Q.5, TP.HCM", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "tuan.hoang@gmail.com", "Hoàng Anh Tuấn", "Bạc", "0934556677", 230 },
                    { 6, "15 Nguyễn Đình Chiểu, Q.3, TP.HCM", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "thao.dang@gmail.com", "Đặng Thu Thảo", "Vàng", "0967889900", 680 },
                    { 7, "102 Cách Mạng Tháng 8, Q.10, TP.HCM", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "bao.vu@gmail.com", "Vũ Quốc Bảo", "Đồng", "0981122334", 80 },
                    { 8, "234 Phan Xích Long, Q.Phú Nhuận, TP.HCM", new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "ngoc.bui@gmail.com", "Bùi Thị Ngọc", "Bạc", "0945667788", 310 },
                    { 9, "56 Võ Văn Tần, Q.3, TP.HCM", new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "khoa.do@gmail.com", "Đỗ Duy Khoa", "Đồng", "0922334455", 15 },
                    { 10, "77 Hoàng Văn Thụ, Q.Phú Nhuận, TP.HCM", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "phuong.ngo@gmail.com", "Ngô Bích Phương", "Vàng", "0918776655", 950 },
                    { 11, "301 Lý Thường Kiệt, Q.11, TP.HCM", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "linh.duong@gmail.com", "Dương Khánh Linh", "Bạc", "0938990011", 110 },
                    { 12, "45 Lê Văn Sỹ, Q.3, TP.HCM", new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "triet.ly@gmail.com", "Lý Minh Triết", "Kim Cương", "0971223344", 1500 },
                    { 13, "89 Nam Kỳ Khởi Nghĩa, Q.1, TP.HCM", new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "thuong.phan@gmail.com", "Phan Hoài Thương", "Đồng", "0908771122", 60 },
                    { 14, "142 Phạm Văn Đồng, Q.Thủ Đức, TP.HCM", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "truong.trinh@gmail.com", "Trịnh Xuân Trường", "Bạc", "0961445566", 420 },
                    { 15, "67 Cộng Hòa, Q.Tân Bình, TP.HCM", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nhi.vo@gmail.com", "Võ Tuyết Nhi", "Vàng", "0949887766", 880 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "Chủ bách hóa (Quản lý toàn bộ kho hàng, nhập hàng, nhân sự và báo cáo doanh thu)", "Admin" },
                    { 2, "Nhân viên thu ngân / POS (Quét mã vạch, lập hóa đơn, nhận thanh toán VietQR/Tiền mặt)", "Thu Ngân" },
                    { 3, "Thành viên thân thiết (Tích điểm, tra cứu hạng thành viên và lịch sử mua hàng)", "Khách Hàng" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "ContactName", "PhoneNumber", "SupplierName" },
                values: new object[,]
                {
                    { 1, "KCN Tân Bình, Q. Tân Phú, TP.HCM", "Nguyễn Văn Hùng", "0901234567", "Công ty Cổ phần Masan Consumer" },
                    { 2, "Phường Bình Hòa, TP. Thuận An, Bình Dương", "Trần Thị Bích", "0912345678", "Công ty TNHH FrieslandCampina Việt Nam" },
                    { 3, "42 Bến Vân Đồn, Q.4, TP.HCM", "Lê Văn Cường", "0923456789", "Tổng Công ty Lương thực Miền Nam (Vinafood II)" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Barcode", "BrandId", "CategoryId", "Price", "ProductName", "StockQuantity", "SupplierId" },
                values: new object[,]
                {
                    { 1, "8938501234561", 1, 1, 12000m, "Snack khoai tây Lay's Vị Tự Nhiên 63g", 100, 1 },
                    { 2, "8938501234562", 1, 2, 20000m, "Nước ngọt Coca-Cola Chai 1.5L", 60, 1 },
                    { 3, "8938501234563", 2, 3, 32000m, "Sữa tươi tiệt trùng Cô Gái Hà Lan Có Đường Lốc 4x180ml", 45, 2 },
                    { 4, "8938501234564", 1, 4, 9000m, "Mì Omachi Xốt Bò Hầm Gói 80g", 200, 1 },
                    { 5, "8938501234565", 1, 5, 45000m, "Nước mắm Chinsu Hương Cá Hồi Chai 500ml", 35, 1 },
                    { 6, "8938501234566", 3, 6, 125000m, "Gạo Thơm Thai Ông Thọ Túi 5kg", 25, 3 },
                    { 7, "8938501234567", 3, 7, 22000m, "Cá sốt cà Hạ Long Hộp 175g", 50, 3 },
                    { 8, "8938501234568", 3, 8, 34000m, "Nước rửa chén Sunlight Chanh Chai 750g", 40, 1 },
                    { 9, "8938501234569", 3, 9, 79000m, "Dầu gội Clear Bạc Hà Chai 340g", 30, 1 },
                    { 10, "8938501234570", 3, 10, 48000m, "Cá viên ngon C.P Gói 500g", 35, 2 },
                    { 11, "8938501234571", 3, 11, 12000m, "Kem Socola Merino Que 60g", 80, 2 },
                    { 12, "8938501234572", 3, 12, 38000m, "Màng bọc thực phẩm Ringo 30cm x 100m", 20, 3 },
                    { 13, "8938501234573", 3, 13, 21000m, "Bia Heineken Lon 330ml", 120, 1 },
                    { 14, "8938501234574", 3, 14, 25000m, "Cà chua Đà Lạt Tươi Túi 1kg", 15, 3 },
                    { 15, "8938501234575", 3, 15, 8000m, "Tập học sinh 96 trang Thiên Long", 150, 3 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "FullName", "PasswordHash", "RoleId", "Username" },
                values: new object[,]
                {
                    { 1, "Quản Lý Hoàng Dung", "admin123", 1, "admin" },
                    { 2, "Thu Ngân Ca Sáng (Nguyễn Thị Mai)", "123456", 2, "thungan01" },
                    { 3, "Thu Ngân Ca Chiều (Trần Quốc Tuấn)", "123456", 2, "thungan02" },
                    { 4, "Lê Hoàng Nam", "123456", 3, "khachhang01" },
                    { 5, "Phạm Thị Lan", "123456", 3, "khachhang02" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "OrderDate", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), "Completed", 32000m, 2 },
                    { 2, 2, new DateTime(2026, 3, 2, 14, 15, 0, 0, DateTimeKind.Unspecified), "Completed", 97000m, 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 12000m },
                    { 2, 1, 2, 1, 20000m },
                    { 3, 2, 5, 1, 45000m },
                    { 4, 2, 13, 2, 21000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
