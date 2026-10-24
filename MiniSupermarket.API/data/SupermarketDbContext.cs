using System;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext(DbContextOptions<SupermarketDbContext> options) : base(options) { }

        // ==========================================
        // KHAI BÁO DANH SÁCH CÁC DBSET (TABLES)
        // ==========================================
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. SEED DATA FOR ROLES (Vai trò hệ thống POS)
            // ==========================================
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Chủ bách hóa (Quản lý toàn bộ kho hàng, nhập hàng, nhân sự và báo cáo doanh thu)" },
                new Role { RoleId = 2, RoleName = "Thu Ngân", Description = "Nhân viên thu ngân / POS (Quét mã vạch, lập hóa đơn, nhận thanh toán VietQR/Tiền mặt)" },
                new Role { RoleId = 3, RoleName = "Khách Hàng", Description = "Thành viên thân thiết (Tích điểm, tra cứu hạng thành viên và lịch sử mua hàng)" }
            );

            // ==========================================
            // 2. SEED DATA FOR USERS (Tài khoản người dùng)
            // ==========================================
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", PasswordHash = "admin123", FullName = "Quản Lý Hoàng Dung", RoleId = 1 },
                new User { UserId = 2, Username = "thungan01", PasswordHash = "123456", FullName = "Thu Ngân Ca Sáng (Nguyễn Thị Mai)", RoleId = 2 },
                new User { UserId = 3, Username = "thungan02", PasswordHash = "123456", FullName = "Thu Ngân Ca Chiều (Trần Quốc Tuấn)", RoleId = 2 },
                new User { UserId = 4, Username = "khachhang01", PasswordHash = "123456", FullName = "Lê Hoàng Nam", RoleId = 3 },
                new User { UserId = 5, Username = "khachhang02", PasswordHash = "123456", FullName = "Phạm Thị Lan", RoleId = 3 }
            );

            // ==========================================
            // 3. SEED DATA FOR SUPPLIERS (Nhà cung cấp bách hóa)
            // ==========================================
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, SupplierName = "Công ty Cổ phần Masan Consumer", ContactName = "Nguyễn Văn Hùng", PhoneNumber = "0901234567", Address = "KCN Tân Bình, Q. Tân Phú, TP.HCM" },
                new Supplier { SupplierId = 2, SupplierName = "Công ty TNHH FrieslandCampina Việt Nam", ContactName = "Trần Thị Bích", PhoneNumber = "0912345678", Address = "Phường Bình Hòa, TP. Thuận An, Bình Dương" },
                new Supplier { SupplierId = 3, SupplierName = "Tổng Công ty Lương thực Miền Nam (Vinafood II)", ContactName = "Lê Văn Cường", PhoneNumber = "0923456789", Address = "42 Bến Vân Đồn, Q.4, TP.HCM" }
            );

            // ==========================================
            // 4. SEED DATA FOR BRANDS (Thương hiệu bách hóa)
            // ==========================================
            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandId = 1, BrandName = "Chinsu / Omachi", Description = "Thương hiệu gia vị và thực phẩm ăn liền nổi tiếng" },
                new Brand { BrandId = 2, BrandName = "Dutch Lady / Cô Gái Hà Lan", Description = "Các sản phẩm sữa tươi và sữa chua dinh dưỡng" },
                new Brand { BrandId = 3, BrandName = "Hoàng Dung Selected", Description = "Thực phẩm khô và nông sản đóng gói nội bộ bách hóa" }
            );

            // ==========================================
            // 5. SEED DATA FOR CATEGORIES (Đủ 15 Danh mục bách hóa)
            // ==========================================
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Bánh kẹo & Đồ ăn vặt", Description = "Snack, bánh quy, kẹo dẻo, sô-cô-la" },
                new Category { CategoryId = 2, CategoryName = "Nước giải khát & Trà", Description = "Nước ngọt, nước khoáng, trà đóng chai, nước ép" },
                new Category { CategoryId = 3, CategoryName = "Sữa & Sản phẩm từ sữa", Description = "Sữa tươi, sữa đặc, sữa chua, phô mai" },
                new Category { CategoryId = 4, CategoryName = "Mì gói & Thực phẩm ăn liền", Description = "Mì ăn liền, phở khô, hủ tiếu, cháo gói" },
                new Category { CategoryId = 5, CategoryName = "Gia vị & Dầu ăn", Description = "Nước mắm, hạt nêm, dầu ăn, xì dầu, bột ngọt" },
                new Category { CategoryId = 6, CategoryName = "Gạo & Nông sản khô", Description = "Gạo ST25, đậu xanh, đậu đen, mộc nhĩ, nấm hương" },
                new Category { CategoryId = 7, CategoryName = "Đồ đóng hộp & Chế biến sẵn", Description = "Cá hộp, thịt xông khói, xúc xích, pate" },
                new Category { CategoryId = 8, CategoryName = "Hóa mỹ phẩm & Giặt xả", Description = "Nước giặt, nước xả vải, nước rửa chén, lau sàn" },
                new Category { CategoryId = 9, CategoryName = "Chăm sóc cá nhân", Description = "Dầu gội, sữa tắm, kem đánh răng, xà phòng" },
                new Category { CategoryId = 10, CategoryName = "Thực phẩm đông lạnh", Description = "Cá viên, bò viên, xúc xích đức, chả giò, bánh xếp" },
                new Category { CategoryId = 11, CategoryName = "Kem & Đồ lạnh", Description = "Kem que, kem hộp, đá viên đóng túi" },
                new Category { CategoryId = 12, CategoryName = "Đồ dùng gia đình gia dụng", Description = "Màng bọc thực phẩm, túi bóng, khăn giấy, mút rửa chén" },
                new Category { CategoryId = 13, CategoryName = "Bia & Đồ uống có cồn", Description = "Bia lon, rượu nhẹ, nước trái cây lên men" },
                new Category { CategoryId = 14, CategoryName = "Rau củ quả tươi (Hàng ngày)", Description = "Rau xanh, củ quả nguyên liệu nấu ăn hằng ngày" },
                new Category { CategoryId = 15, CategoryName = "Văn phòng phẩm & Đồ chơi nhỏ", Description = "Bút, tập học sinh, đồ chơi trẻ em cơ bản" }
            );

            // ==========================================
            // 6. SEED DATA FOR PRODUCTS (Đủ 15 Sản phẩm cho 15 Danh mục)
            // ==========================================
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Barcode = "8938501234561", ProductName = "Snack khoai tây Lay's Vị Tự Nhiên 63g", Price = 12000, StockQuantity = 100, CategoryId = 1, BrandId = 1, SupplierId = 1 },
                new Product { ProductId = 2, Barcode = "8938501234562", ProductName = "Nước ngọt Coca-Cola Chai 1.5L", Price = 20000, StockQuantity = 60, CategoryId = 2, BrandId = 1, SupplierId = 1 },
                new Product { ProductId = 3, Barcode = "8938501234563", ProductName = "Sữa tươi tiệt trùng Cô Gái Hà Lan Có Đường Lốc 4x180ml", Price = 32000, StockQuantity = 45, CategoryId = 3, BrandId = 2, SupplierId = 2 },
                new Product { ProductId = 4, Barcode = "8938501234564", ProductName = "Mì Omachi Xốt Bò Hầm Gói 80g", Price = 9000, StockQuantity = 200, CategoryId = 4, BrandId = 1, SupplierId = 1 },
                new Product { ProductId = 5, Barcode = "8938501234565", ProductName = "Nước mắm Chinsu Hương Cá Hồi Chai 500ml", Price = 45000, StockQuantity = 35, CategoryId = 5, BrandId = 1, SupplierId = 1 },
                new Product { ProductId = 6, Barcode = "8938501234566", ProductName = "Gạo Thơm Thai Ông Thọ Túi 5kg", Price = 125000, StockQuantity = 25, CategoryId = 6, BrandId = 3, SupplierId = 3 },
                new Product { ProductId = 7, Barcode = "8938501234567", ProductName = "Cá sốt cà Hạ Long Hộp 175g", Price = 22000, StockQuantity = 50, CategoryId = 7, BrandId = 3, SupplierId = 3 },
                new Product { ProductId = 8, Barcode = "8938501234568", ProductName = "Nước rửa chén Sunlight Chanh Chai 750g", Price = 34000, StockQuantity = 40, CategoryId = 8, BrandId = 3, SupplierId = 1 },
                new Product { ProductId = 9, Barcode = "8938501234569", ProductName = "Dầu gội Clear Bạc Hà Chai 340g", Price = 79000, StockQuantity = 30, CategoryId = 9, BrandId = 3, SupplierId = 1 },
                new Product { ProductId = 10, Barcode = "8938501234570", ProductName = "Cá viên ngon C.P Gói 500g", Price = 48000, StockQuantity = 35, CategoryId = 10, BrandId = 3, SupplierId = 2 },
                new Product { ProductId = 11, Barcode = "8938501234571", ProductName = "Kem Socola Merino Que 60g", Price = 12000, StockQuantity = 80, CategoryId = 11, BrandId = 3, SupplierId = 2 },
                new Product { ProductId = 12, Barcode = "8938501234572", ProductName = "Màng bọc thực phẩm Ringo 30cm x 100m", Price = 38000, StockQuantity = 20, CategoryId = 12, BrandId = 3, SupplierId = 3 },
                new Product { ProductId = 13, Barcode = "8938501234573", ProductName = "Bia Heineken Lon 330ml", Price = 21000, StockQuantity = 120, CategoryId = 13, BrandId = 3, SupplierId = 1 },
                new Product { ProductId = 14, Barcode = "8938501234574", ProductName = "Cà chua Đà Lạt Tươi Túi 1kg", Price = 25000, StockQuantity = 15, CategoryId = 14, BrandId = 3, SupplierId = 3 },
                new Product { ProductId = 15, Barcode = "8938501234575", ProductName = "Tập học sinh 96 trang Thiên Long", Price = 8000, StockQuantity = 150, CategoryId = 15, BrandId = 3, SupplierId = 3 }
            );

            // ==========================================
            // 7. SEED DATA FOR CUSTOMERS (15 Khách hàng thân thiết)
            // ==========================================
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FullName = "Lê Hoàng Nam", PhoneNumber = "0987654321", Email = "nam.le@gmail.com", Address = "123 Lê Lợi, Q.1, TP.HCM", RewardPoints = 150, MembershipRank = "Bạc", CreatedAt = new DateTime(2026, 1, 15) },
                new Customer { CustomerId = 2, FullName = "Phạm Thị Lan", PhoneNumber = "0978123456", Email = "lan.pham@gmail.com", Address = "456 Nguyễn Thị Minh Khai, Q.3, TP.HCM", RewardPoints = 520, MembershipRank = "Vàng", CreatedAt = new DateTime(2026, 2, 10) },
                new Customer { CustomerId = 3, FullName = "Trần Văn Minh", PhoneNumber = "0912345678", Email = "minh.tran@gmail.com", Address = "789 Điện Biên Phủ, Q.Bình Thạnh, TP.HCM", RewardPoints = 1200, MembershipRank = "Kim Cương", CreatedAt = new DateTime(2026, 2, 18) },
                new Customer { CustomerId = 4, FullName = "Lê Thị Thanh", PhoneNumber = "0903112233", Email = "thanh.le@gmail.com", Address = "12 Trần Hưng Đạo, Q.5, TP.HCM", RewardPoints = 45, MembershipRank = "Đồng", CreatedAt = new DateTime(2026, 3, 1) },
                new Customer { CustomerId = 5, FullName = "Hoàng Anh Tuấn", PhoneNumber = "0934556677", Email = "tuan.hoang@gmail.com", Address = "88 Nguyễn Trãi, Q.5, TP.HCM", RewardPoints = 230, MembershipRank = "Bạc", CreatedAt = new DateTime(2026, 3, 5) },
                new Customer { CustomerId = 6, FullName = "Đặng Thu Thảo", PhoneNumber = "0967889900", Email = "thao.dang@gmail.com", Address = "15 Nguyễn Đình Chiểu, Q.3, TP.HCM", RewardPoints = 680, MembershipRank = "Vàng", CreatedAt = new DateTime(2026, 3, 12) },
                new Customer { CustomerId = 7, FullName = "Vũ Quốc Bảo", PhoneNumber = "0981122334", Email = "bao.vu@gmail.com", Address = "102 Cách Mạng Tháng 8, Q.10, TP.HCM", RewardPoints = 80, MembershipRank = "Đồng", CreatedAt = new DateTime(2026, 3, 20) },
                new Customer { CustomerId = 8, FullName = "Bùi Thị Ngọc", PhoneNumber = "0945667788", Email = "ngoc.bui@gmail.com", Address = "234 Phan Xích Long, Q.Phú Nhuận, TP.HCM", RewardPoints = 310, MembershipRank = "Bạc", CreatedAt = new DateTime(2026, 4, 2) },
                new Customer { CustomerId = 9, FullName = "Đỗ Duy Khoa", PhoneNumber = "0922334455", Email = "khoa.do@gmail.com", Address = "56 Võ Văn Tần, Q.3, TP.HCM", RewardPoints = 15, MembershipRank = "Đồng", CreatedAt = new DateTime(2026, 4, 15) },
                new Customer { CustomerId = 10, FullName = "Ngô Bích Phương", PhoneNumber = "0918776655", Email = "phuong.ngo@gmail.com", Address = "77 Hoàng Văn Thụ, Q.Phú Nhuận, TP.HCM", RewardPoints = 950, MembershipRank = "Vàng", CreatedAt = new DateTime(2026, 5, 1) },
                new Customer { CustomerId = 11, FullName = "Dương Khánh Linh", PhoneNumber = "0938990011", Email = "linh.duong@gmail.com", Address = "301 Lý Thường Kiệt, Q.11, TP.HCM", RewardPoints = 110, MembershipRank = "Bạc", CreatedAt = new DateTime(2026, 5, 10) },
                new Customer { CustomerId = 12, FullName = "Lý Minh Triết", PhoneNumber = "0971223344", Email = "triet.ly@gmail.com", Address = "45 Lê Văn Sỹ, Q.3, TP.HCM", RewardPoints = 1500, MembershipRank = "Kim Cương", CreatedAt = new DateTime(2026, 5, 22) },
                new Customer { CustomerId = 13, FullName = "Phan Hoài Thương", PhoneNumber = "0908771122", Email = "thuong.phan@gmail.com", Address = "89 Nam Kỳ Khởi Nghĩa, Q.1, TP.HCM", RewardPoints = 60, MembershipRank = "Đồng", CreatedAt = new DateTime(2026, 6, 5) },
                new Customer { CustomerId = 14, FullName = "Trịnh Xuân Trường", PhoneNumber = "0961445566", Email = "truong.trinh@gmail.com", Address = "142 Phạm Văn Đồng, Q.Thủ Đức, TP.HCM", RewardPoints = 420, MembershipRank = "Bạc", CreatedAt = new DateTime(2026, 6, 18) },
                new Customer { CustomerId = 15, FullName = "Võ Tuyết Nhi", PhoneNumber = "0949887766", Email = "nhi.vo@gmail.com", Address = "67 Cộng Hòa, Q.Tân Bình, TP.HCM", RewardPoints = 880, MembershipRank = "Vàng", CreatedAt = new DateTime(2026, 7, 1) }
            );

            // ==========================================
            // 8. SEED DATA FOR ORDERS (Đơn hàng POS mẫu)
            // ==========================================
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, OrderDate = new DateTime(2026, 3, 1, 10, 30, 0), TotalAmount = 32000, Status = "Completed", CustomerId = 1, UserId = 2 },
                new Order { OrderId = 2, OrderDate = new DateTime(2026, 3, 2, 14, 15, 0), TotalAmount = 97000, Status = "Completed", CustomerId = 2, UserId = 2 }
            );

            // ==========================================
            // 9. SEED DATA FOR ORDERITEMS (Chi tiết đơn hàng)
            // ==========================================
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 12000 },
                new OrderItem { OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 20000 },
                new OrderItem { OrderItemId = 3, OrderId = 2, ProductId = 5, Quantity = 1, UnitPrice = 45000 },
                new OrderItem { OrderItemId = 4, OrderId = 2, ProductId = 13, Quantity = 2, UnitPrice = 21000 }
            );
        }
    }
}