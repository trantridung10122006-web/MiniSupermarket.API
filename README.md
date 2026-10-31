# MiniSupermarket

## 1. Kiến trúc Solution

Solution gồm đúng hai project độc lập:

- `MiniSupermarket.API`: ASP.NET Core Web API, .NET 8.
- `MiniSupermarket.WinForms`: Windows Forms Client, .NET Framework 4.8.

API đăng ký DbContext, JWT Authentication, Authorization, Swagger và CORS trong `Program.cs`. WinForms gọi API bằng `HttpClient` qua `http://localhost:5000/api/` mặc định.

## 2. Cấu hình và chạy

1. Mở `MiniSupermarket.API.sln`.
2. Kiểm tra SQL Server và `MiniSupermarket.API/appsettings.json`. Chuỗi kết nối nằm tại `ConnectionStrings:DefaultConnection`, JWT secret tại `JwtSettings:Secret`.
3. Không hard-code chuỗi kết nối hoặc JWT secret trong C#. Khi triển khai thật, dùng User Secrets, biến môi trường hoặc Secret Manager.
4. Chọn `MiniSupermarket.API` làm Startup Project và nhấn F5.
5. Mở Swagger tại `http://localhost:5000/swagger` hoặc URL trong Output.
6. Chạy `MiniSupermarket.WinForms` sau khi API đã hoạt động.

## 3. Migration, database và seed data

Trong Package Manager Console, chọn `MiniSupermarket.API` làm Default project:

```powershell
Add-Migration TenMigration
Update-Database
```

API cũng gọi `Database.Migrate()` khi khởi động. Migration hiện có bao gồm `RenameOrderItemToOrderDetail`, giữ nguyên bảng SQL `OrderItems` và đổi tên khóa thành `OrderDetailId`. DbContext seed tối thiểu 15 danh mục và 15 sản phẩm cùng user, role, customer, order và order detail mẫu.

Stored procedure `sp_GetRevenueByCashier` được tạo hoặc cập nhật khi API khởi động.

## 4. Tài khoản kiểm thử

| Username | Password | Role |
|---|---|---|
| `admin` | `admin123` | Admin |
| `thungan01` | `123456` | Cashier |
| `thungan02` | `123456` | Cashier |
| `khachhang01` | `123456` | Customer |

Đây là tài khoản học tập, không dùng nguyên trạng trong môi trường production.

## 5. Kiểm thử JWT trên Swagger

1. Gọi `POST /api/Auth/login` với username và password mẫu.
2. Sao chép `token` từ response.
3. Nhấn **Authorize** trên Swagger.
4. Nhập `Bearer <token>`, nhấn **Authorize**, sau đó gọi API bảo mật.

Kết quả mong đợi: `200` khi thành công, `400` khi dữ liệu sai, `401` khi chưa đăng nhập, `403` khi không đủ quyền và `404` khi không tìm thấy dữ liệu.

## 6. Tiêu chí kỹ thuật 11–35

### 11–17. API, Controller và HTTP

- Có đúng hai project Backend API và WinForms Client.
- Có `AuthController`, `CategoriesController`, `ProductsController`, cùng các controller Order, Customer, User và Report.
- Route RESTful: `/api/Categories`, `/api/Products`, `/api/Orders`.
- Dùng đúng GET, POST, PUT, DELETE và status code chuẩn.
- Controller dùng `async Task<IActionResult>` và EF Core async API.
- Swagger UI có nút Authorize để kiểm thử JWT.
- CORS được bật cho WinForms gọi API.

### 18–22. Entity, DbContext, validation, migration và seed

- Entity chính: `User`, `Category`, `Product`, `Customer`, `Order`, `OrderDetail`.
- `SupermarketDbContext` khai báo DbSet và cấu hình quan hệ Category–Product, Order–OrderDetail, Product–OrderDetail.
- Dùng `[Required]`, `[StringLength]`, `[Range]` cho dữ liệu bắt buộc, độ dài chuỗi, giá bán và tồn kho.
- Dùng `Add-Migration` và `Update-Database` để quản lý schema.
- Có seed tối thiểu 5 nhóm hàng và 15 sản phẩm; project hiện có 15 nhóm hàng và 15 sản phẩm.

### 23–27. CRUD, LINQ, phân trang và truy vấn đọc

- Category và Product có đủ CRUD qua EF Core.
- Tìm kiếm sử dụng LINQ `Contains()`.
- Lọc sản phẩm theo `categoryId`.
- Phân trang dùng `.Skip()` và `.Take()` tại `/api/Products/paged`.
- Truy vấn Product dùng `.Include(p => p.Category)` để trả `CategoryName`.
- Truy vấn GET chỉ đọc dùng `.AsNoTracking()`.
- Request/response chính sử dụng DTO, không trả trực tiếp Entity CSDL.

### 28–31. JWT và phân quyền

- `POST /api/Auth/login` trả JWT hợp lệ.
- API bảo mật gắn `[Authorize]`.
- Admin toàn quyền quản trị.
- Admin và Cashier được phép thực hiện nghiệp vụ POS/thanh toán.
- Reports, User và thao tác xóa quản trị được giới hạn cho Admin.
- JWT được gắn trong Swagger bằng nút Authorize.

### 32–34. ADO.NET và Stored Procedure

- Tra cứu Barcode dùng `SqlConnection`, `SqlCommand` và `SqlDataReader`.
- Truy vấn dùng tham số, không nối chuỗi SQL; endpoint ADO dùng `AddWithValue`.
- Endpoint `GET /api/Reports/shift-revenue` gọi `sp_GetRevenueByCashier` bằng `CommandType.StoredProcedure`.

### 35. Thanh toán và Transaction

`POST /api/Orders/checkout` thực hiện trong một `SqlTransaction`:

1. Khóa và kiểm tra tồn kho bằng `UPDLOCK`.
2. Tạo hóa đơn và chi tiết hóa đơn.
3. Trừ tồn kho.
4. Cộng điểm khách hàng.
5. `CommitAsync()` khi toàn bộ nghiệp vụ thành công.
6. `RollbackAsync()` khi hết hàng hoặc có lỗi, không tạo hóa đơn rác.

## 7. Endpoint kiểm thử nhanh

```text
POST /api/Auth/login
GET  /api/Categories
GET  /api/Products
GET  /api/Products/search?keyword=milk
GET  /api/Products/category/1
GET  /api/Products/paged?pageNumber=1&pageSize=10
GET  /api/products/barcode-ado/{barcode}
GET  /api/Reports/shift-revenue
POST /api/Orders/checkout
```

## 8. Checklist trước khi nộp

- [ ] Build Solution thành công.
- [ ] `Update-Database` chạy thành công.
- [ ] Swagger mở được và login trả JWT.
- [ ] API không có token trả `401`.
- [ ] Role không phù hợp trả `403`.
- [ ] CRUD, tìm kiếm, lọc và phân trang hoạt động.
- [ ] Barcode ADO.NET trả đúng sản phẩm.
- [ ] Stored Procedure trả đúng doanh thu.
- [ ] Thanh toán thiếu hàng rollback và không lưu hóa đơn rác.

## 9. Đường dẫn minh chứng để báo cáo giảng viên

Khi trình bày, có thể mở các file sau để chứng minh từng nhóm tiêu chí:

| Tiêu chí | Nội dung báo cáo | Đường dẫn minh chứng |
|---|---|---|
| 11 | Solution gồm API và WinForms; cấu hình dịch vụ, Swagger, CORS | `MiniSupermarket.API.sln`; `MiniSupermarket.API/Program.cs`; `MiniSupermarket.API/MiniSupermarket.API.csproj`; `MiniSupermarket.WinForms/MiniSupermarket.WinForms.csproj` |
| 12 | Chuỗi kết nối nằm trong JSON | `MiniSupermarket.API/appsettings.json` |
| 13–17 | Controller, route RESTful, HTTP method, status code, DTO, async, Swagger | `MiniSupermarket.API/Controllers/AuthController.cs`; `MiniSupermarket.API/Controllers/CategoriesController.cs`; `MiniSupermarket.API/Controllers/ProductController.cs`; `MiniSupermarket.API/model/CategoryDtos.cs`; `MiniSupermarket.API/model/ProductDtos.cs`; `MiniSupermarket.API/Program.cs` |
| 18 | Sáu Entity chính | `MiniSupermarket.API/model/User.cs`; `MiniSupermarket.API/model/Category.cs`; `MiniSupermarket.API/model/Product.cs`; `MiniSupermarket.API/model/Customer.cs`; `MiniSupermarket.API/model/Order.cs`; `MiniSupermarket.API/model/OrderDetail.cs` |
| 19 | DbSet và khóa ngoại | `MiniSupermarket.API/data/SupermarketDbContext.cs`; `MiniSupermarket.API/model/Category.cs`; `MiniSupermarket.API/model/Product.cs`; `MiniSupermarket.API/model/Order.cs`; `MiniSupermarket.API/model/OrderDetail.cs` |
| 20 | Required, StringLength, Range | `MiniSupermarket.API/model/Product.cs`; `MiniSupermarket.API/model/Category.cs`; `MiniSupermarket.API/model/Customer.cs`; `MiniSupermarket.API/model/OrderDetail.cs`; `MiniSupermarket.API/model/ProductDtos.cs` |
| 21–22 | Migration và seed data | `MiniSupermarket.API/Migrations/`; `MiniSupermarket.API/data/SupermarketDbContext.cs` |
| 23 | CRUD Category/Product/Customer | `MiniSupermarket.API/Controllers/CategoriesController.cs`; `MiniSupermarket.API/Controllers/ProductController.cs`; `MiniSupermarket.API/Controllers/CustomerController.cs` |
| 24–27 | LINQ search, lọc, phân trang, Include, AsNoTracking | `MiniSupermarket.API/Controllers/ProductController.cs`; `MiniSupermarket.API/Controllers/CategoriesController.cs`; `MiniSupermarket.API/Controllers/CustomerController.cs` |
| 28–31 | JWT, Authorize, Role và Swagger Authorize | `MiniSupermarket.API/Controllers/AuthController.cs`; `MiniSupermarket.API/Controllers/UserController.cs`; `MiniSupermarket.API/Controllers/OrdersController.cs`; `MiniSupermarket.API/Controllers/ReportsController.cs`; `MiniSupermarket.API/Program.cs` |
| 32–33 | Barcode ADO.NET, SqlDataReader, tham số SQL | `MiniSupermarket.API/Controllers/AdoProductsController.cs` |
| 34 | Stored Procedure báo cáo doanh thu | `MiniSupermarket.API/Controllers/ReportsController.cs`; `MiniSupermarket.API/sp_GetRevenueByCashier.sql`; `MiniSupermarket.API/Program.cs` |
| 35 | Transaction, kiểm tra tồn kho, commit và rollback | `MiniSupermarket.API/Controllers/OrdersController.cs` |

### Các đường dẫn nên mở trực tiếp khi demo

1. `MiniSupermarket.API/Program.cs`: trình bày đăng ký DbContext, JWT, Swagger, CORS và middleware.
2. `MiniSupermarket.API/Controllers/AuthController.cs`: trình bày đăng nhập và tạo JWT.
3. `MiniSupermarket.API/Controllers/ProductController.cs`: trình bày DTO, CRUD, tìm kiếm, lọc, phân trang, Include và AsNoTracking.
4. `MiniSupermarket.API/data/SupermarketDbContext.cs`: trình bày DbSet, quan hệ, validation liên quan và seed data.
5. `MiniSupermarket.API/Controllers/AdoProductsController.cs`: trình bày truy vấn Barcode bằng ADO.NET.
6. `MiniSupermarket.API/Controllers/ReportsController.cs`: trình bày gọi Stored Procedure.
7. `MiniSupermarket.API/Controllers/OrdersController.cs`: trình bày thanh toán, trừ kho, tích điểm và rollback transaction.
8. `MiniSupermarket.API/Migrations/`: trình bày lịch sử tạo và cập nhật CSDL.
9. `MiniSupermarket.WinForms/SessionManager.cs`: trình bày WinForms lưu token và gắn Bearer token khi gọi API.

### Minh chứng chạy thực tế trên Swagger

- Login: `POST /api/Auth/login`.
- Danh sách nhóm hàng: `GET /api/Categories`.
- Danh sách sản phẩm: `GET /api/Products`.
- Tìm kiếm: `GET /api/Products/search?keyword=...`.
- Phân trang: `GET /api/Products/paged?pageNumber=1&pageSize=10`.
- Barcode ADO.NET: `GET /api/products/barcode-ado/{barcode}`.
- Báo cáo ca: `GET /api/Reports/shift-revenue`.
- Thanh toán: `POST /api/Orders/checkout`.

Sau khi login, nhấn **Authorize** và nhập `Bearer <token>` trước khi gọi các endpoint yêu cầu đăng nhập.

## 10. Tiêu chí WinForms 36–50 và đường dẫn minh chứng

### 36–40. Shell, đăng nhập, phân quyền và HttpClient

- **36–37 Shell và nạp Form con:** `MiniSupermarket.WinForms/FormMainShell.cs`.
  - `panelSidebar.Dock = DockStyle.Left`.
  - `panelTopHeader.Dock = DockStyle.Top`.
  - `panelMainContent.Dock = DockStyle.Fill`.
  - `OpenChildForm()` đặt `TopLevel = false`, `FormBorderStyle = None`, `Dock = Fill`.
- **38 Lưu JWT Token:** `MiniSupermarket.WinForms/FormLogin.cs` và `MiniSupermarket.WinForms/SessionManager.cs`.
- **39 Ẩn/hiện menu theo quyền:** `MiniSupermarket.WinForms/FormMainShell.cs`, hàm `ApplyRolePermissions()`.
- **40 HttpClient dùng chung:** `MiniSupermarket.WinForms/SessionManager.cs`, class `ApiClientService`; property `Client` tự gắn `Bearer Token`.

### 41–43. Danh mục, POS và cảnh báo tồn kho

- **41 DataGridView danh mục:** `MiniSupermarket.WinForms/FormCategory.cs`, `FormCategory.Designer.cs`, `FormCategoryManagement.cs`.
- **42 POS và thông báo lỗi:** `MiniSupermarket.WinForms/FormPOS.cs`; luồng quét Barcode, cộng dồn giỏ hàng, tính tiền thừa và hiển thị lỗi tiếng Việt.
- **43 Low-stock alert:** `MiniSupermarket.WinForms/FormPOS.cs`, hàm `SetupProductGrid()` và sự kiện `CellFormatting`.
  - Tồn kho `<= 10`: nền `Moccasin`.
  - Tồn kho `<= 0`: nền `LightCoral`.

### 44–46. Dashboard, doanh thu và sản phẩm bán chạy

- **44 Dashboard KPI:**
  - API: `MiniSupermarket.API/Controllers/ReportsController.cs`, endpoint `GET /api/Reports/dashboard-summary`.
  - DTO: `MiniSupermarket.API/model/ReportDtos.cs`, class `DashboardSummaryDto`.
  - WinForms: `MiniSupermarket.WinForms/FormReports.cs`, bốn thẻ doanh thu hôm nay, hóa đơn, khách mới và hàng sắp hết.
- **45 Doanh thu theo ngày:**
  - API: `GET /api/Reports/revenue-trend`.
  - Code gom nhóm ngày: `MiniSupermarket.API/Controllers/ReportsController.cs`, hàm `GetRevenueTrend()`.
  - Bộ lọc thời gian và báo cáo: `MiniSupermarket.WinForms/FormReports.cs`.
- **46 Top 5 bán chạy:**
  - API: `GET /api/Reports/best-sellers`.
  - LINQ `GroupBy`, `OrderByDescending`, `Take(5)`: `MiniSupermarket.API/Controllers/ReportsController.cs`.
  - Hiển thị danh sách Top 5: `MiniSupermarket.WinForms/FormReports.cs`.

### 47–50. Loyalty, xuất file và demo bảo vệ

- **47 Đổi điểm trừ hóa đơn:**
  - Giao diện nhập điểm: `MiniSupermarket.WinForms/FormPOS.cs`, control `nudRedeemPoints`.
  - Request gửi `RedeemPoints`: `MiniSupermarket.WinForms/FormPOS.cs`.
  - Trừ điểm, giảm tiền, cộng điểm mới trong cùng transaction: `MiniSupermarket.API/Controllers/OrdersController.cs`.
  - Quy đổi: 1 điểm = 1.000 VNĐ.
- **48 Xuất báo cáo:** `MiniSupermarket.WinForms/FormReports.cs`, hàm `ExportReport()`, `ExportToCsv()` và `ExportToHtmlExcel()`.
- **49 Demo trơn tru:** chạy API trước, đăng nhập WinForms, mở POS, quét Barcode, thanh toán, sau đó kiểm tra lại đơn hàng/tồn kho trên SQL Server.
- **50 Vấn đáp:** khi giải thích, mở lần lượt `FormMainShell.cs`, `FormLogin.cs`, `SessionManager.cs`, `FormPOS.cs`, `ReportsController.cs`, `OrdersController.cs` và `SupermarketDbContext.cs` để chỉ luồng từ giao diện → API → transaction → CSDL.

### Endpoint mới cần demo

```text
GET  /api/Reports/dashboard-summary
GET  /api/Reports/revenue-trend?fromDate=2026-01-01&toDate=2026-12-31
GET  /api/Reports/best-sellers?fromDate=2026-01-01&toDate=2026-12-31
POST /api/Orders/checkout  (kèm RedeemPoints)
```

## 11. Bảng chỉ đúng đoạn code cần mở khi báo cáo

| Tiêu chí | Mở file | Tìm class / method / dòng lệnh |
|---|---|---|
| 11 | `MiniSupermarket.API/Program.cs` | `AddDbContext`, `AddCors`, `AddAuthentication`, `AddSwaggerGen`, `UseCors`, `MapControllers` |
| 12 | `MiniSupermarket.API/appsettings.json` | `ConnectionStrings:DefaultConnection` |
| 13 | `MiniSupermarket.API/Controllers/AuthController.cs`, `CategoriesController.cs`, `ProductController.cs` | `[Route]`, `AuthController`, `CategoriesController`, `ProductsController` |
| 14 | `MiniSupermarket.API/Controllers/CategoriesController.cs`, `ProductController.cs` | `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`, `Ok`, `CreatedAtAction`, `BadRequest`, `NotFound` |
| 15 | `MiniSupermarket.API/model/CategoryDtos.cs`, `ProductDtos.cs`, `CustomerDtos.cs` | `CategoryDto`, `ProductDto`, `ProductRequestDto`, `FromEntity` |
| 16 | `MiniSupermarket.API/Controllers` | Các method `async Task<IActionResult>`, `ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync` |
| 17 | `MiniSupermarket.API/Program.cs` | `AddSwaggerGen`, `AddSecurityDefinition`, `AddSecurityRequirement` |
| 18 | `MiniSupermarket.API/model` | `User`, `Category`, `Product`, `Customer`, `Order`, `OrderDetail` |
| 19 | `MiniSupermarket.API/data/SupermarketDbContext.cs` | `DbSet`, `HasMany`, `WithOne`, `HasForeignKey`, `OnDelete` |
| 20 | `MiniSupermarket.API/model/Product.cs`, `Category.cs`, `OrderDetail.cs` | `[Required]`, `[StringLength]`, `[Range]` |
| 21 | `MiniSupermarket.API/Migrations/` | Migration `RenameOrderItemToOrderDetail` và `SupermarketDbContextModelSnapshot` |
| 22 | `MiniSupermarket.API/data/SupermarketDbContext.cs` | `HasData`, phần seed Category/Product |
| 23 | `MiniSupermarket.API/Controllers/CategoriesController.cs`, `ProductController.cs` | `Create`, `Update`, `Delete` |
| 24 | `MiniSupermarket.API/Controllers/ProductController.cs` | `Search`, `GetByCategory`, `.Contains()` |
| 25 | `MiniSupermarket.API/Controllers/ProductController.cs` | `GetPaged`, `.Skip()`, `.Take()` |
| 26 | `MiniSupermarket.API/Controllers/ProductController.cs` | `.Include(p => p.Category)` và `ProductDto.FromEntity` |
| 27 | `MiniSupermarket.API/Controllers/ProductController.cs` | `.AsNoTracking()` trong `GetAll`, `GetPaged`, `GetById` |
| 28 | `MiniSupermarket.API/Controllers/AuthController.cs` | `Login`, `GenerateJwtToken` |
| 29 | `MiniSupermarket.API/Controllers/ProductController.cs`, `OrdersController.cs` | `[Authorize]` |
| 30 | `MiniSupermarket.API/Controllers/CategoriesController.cs`, `UserController.cs`, `ReportsController.cs` | `[Authorize(Roles = "Admin")]`, `[Authorize(Roles = "Admin,Cashier")]` |
| 31 | `MiniSupermarket.API/Program.cs` | Swagger security scheme `Bearer` |
| 32–33 | `MiniSupermarket.API/Controllers/AdoProductsController.cs` | `GetByBarcode`, `SqlConnection`, `SqlCommand`, `AddWithValue`, `ExecuteReaderAsync`, `ReadAsync` |
| 34 | `MiniSupermarket.API/Controllers/ReportsController.cs` | `GetShiftRevenue`, `CommandType.StoredProcedure` |
| 35 | `MiniSupermarket.API/Controllers/OrdersController.cs` | `Checkout`, `BeginTransaction`, `UPDLOCK`, `CommitAsync`, `RollbackAsync` |
| 36–37 | `MiniSupermarket.WinForms/FormMainShell.cs` | `panelSidebar`, `panelTopHeader`, `panelMainContent`, `OpenChildForm` |
| 38 | `MiniSupermarket.WinForms/FormLogin.cs`, `SessionManager.cs` | `btnLogin_Click`, `JwtToken`, `LoginAsync` |
| 39 | `MiniSupermarket.WinForms/FormMainShell.cs` | `ApplyRolePermissions` |
| 40 | `MiniSupermarket.WinForms/SessionManager.cs` | `ApiClientService.Client`, `AuthenticationHeaderValue("Bearer", ...)` |
| 41 | `MiniSupermarket.WinForms/FormCategory.cs` | `LoadCategoriesAsync`, `dgvCategories`, các nút Create/Update/Delete |
| 42 | `MiniSupermarket.WinForms/FormPOS.cs` | `AddProductToCart`, `UpdateCartDisplay`, `CalculateChange`, `btnCheckout_Click` |
| 43 | `MiniSupermarket.WinForms/FormPOS.cs` | `SetupProductGrid`, `dgvProducts.CellFormatting`, `StockQuantity <= 10` |
| 44 | `MiniSupermarket.API/Controllers/ReportsController.cs`, `FormReports.cs` | `GetDashboardSummary`, `DashboardSummaryDto`, `LoadDashboardExtrasAsync` |
| 45 | `MiniSupermarket.API/Controllers/ReportsController.cs` | `GetRevenueTrend`, `RevenueTrendDto`, `GroupBy(o => o.OrderDate.Date)` |
| 46 | `MiniSupermarket.API/Controllers/ReportsController.cs`, `FormReports.cs` | `GetBestSellers`, `BestSellerDto`, `OrderByDescending`, `Take(5)` |
| 47 | `MiniSupermarket.API/Controllers/OrdersController.cs`, `FormPOS.cs` | `RedeemPoints`, `discountAmount`, `currentRewardPoints`, `nudRedeemPoints` |
| 48 | `MiniSupermarket.WinForms/FormReports.cs` | `ExportReport`, `ExportToCsv`, `ExportToHtmlExcel` |
| 49 | `MiniSupermarket.WinForms/FormPOS.cs` | `btnCheckout_Click`; sau đó kiểm tra `Orders` và `Products` trên SQL Server |
| 50 | Các file ở bảng trên | Trình bày luồng `Form → ApiClientService → Controller → DbContext/SqlTransaction → SQL Server` |

**Mẹo khi báo cáo:** dùng `Ctrl+T` hoặc `Ctrl+F` trong Visual Studio để tìm đúng tên class/method ở cột cuối, sau đó chạy endpoint tương ứng trong Swagger để chứng minh kết quả.

## 12. Cách trình bày code khi bảo vệ

Phần này là kịch bản nói ngắn gọn. Không cần đọc toàn bộ code; chỉ mở đúng file, tìm đúng method và giải thích luồng xử lý.

### 12.1. Giới thiệu tổng quan

Có thể nói:

> Đề tài gồm hai phần. `MiniSupermarket.API` là Web API viết bằng ASP.NET Core .NET 8, chịu trách nhiệm xử lý nghiệp vụ và làm việc với SQL Server. `MiniSupermarket.WinForms` là giao diện bán hàng viết bằng Windows Forms .NET Framework 4.8, gọi API bằng `HttpClient`.

Mở solution để chỉ hai project. Sau đó mở `Program.cs` và nói:

> Đây là nơi cấu hình toàn bộ API: kết nối cơ sở dữ liệu, JWT đăng nhập, phân quyền, Swagger và CORS.

### 12.2. Trình bày chức năng đăng nhập

Mở `MiniSupermarket.API/Controllers/AuthController.cs`, tìm `Login` và `GenerateJwtToken`:

> Người dùng gửi username và password đến endpoint login. Nếu thông tin đúng, API tạo JWT và trả token về cho client. Token chứa username và role, nhờ đó các endpoint khác biết người dùng là Admin, Cashier hay Customer.

Chạy `POST /api/Auth/login` trên Swagger. Sao chép token, nhấn **Authorize** và nhập `Bearer <token>`.

### 12.3. Trình bày Category và Product

Mở `MiniSupermarket.API/Controllers/ProductController.cs` và chỉ lần lượt các method `GetAll`, `Create`, `Update`, `Delete`, `Search`, `GetByCategory` và `GetPaged`:

> Controller nhận request từ client, kiểm tra dữ liệu, gọi Entity Framework Core để truy vấn SQL Server rồi trả kết quả bằng HTTP status code. `Contains` dùng để tìm kiếm, `Skip` và `Take` dùng để phân trang. `Include` lấy thêm thông tin Category. `AsNoTracking` được dùng cho truy vấn chỉ đọc để không cần theo dõi entity.

Sau đó chạy các endpoint `GET /api/Products`, tìm kiếm và phân trang trên Swagger để chứng minh kết quả.

### 12.4. Trình bày Entity, DbContext và dữ liệu mẫu

Mở `MiniSupermarket.API/data/SupermarketDbContext.cs`:

> `DbSet` đại diện cho các bảng trong cơ sở dữ liệu. `HasMany`, `WithOne` và `HasForeignKey` mô tả quan hệ giữa Category–Product và Order–OrderDetail. `HasData` tạo dữ liệu mẫu khi khởi tạo cơ sở dữ liệu. Các thuộc tính `[Required]`, `[StringLength]` và `[Range]` dùng để kiểm tra dữ liệu đầu vào.

Mở thư mục `Migrations/` để giải thích:

> Migration lưu lại lịch sử thay đổi cấu trúc cơ sở dữ liệu. `Update-Database` áp dụng các thay đổi đó vào SQL Server.

### 12.5. Trình bày ADO.NET và Stored Procedure

Mở `MiniSupermarket.API/Controllers/AdoProductsController.cs`:

> Chức năng này dùng ADO.NET để tìm sản phẩm theo Barcode. `SqlConnection` mở kết nối, `SqlCommand` thực hiện câu lệnh, còn `SqlDataReader` đọc kết quả. Giá trị Barcode được truyền bằng tham số nên không nối trực tiếp dữ liệu người dùng vào câu SQL.

Mở `ReportsController.cs` và tìm `GetShiftRevenue`:

> Báo cáo doanh thu gọi Stored Procedure `sp_GetRevenueByCashier`. Cách này gom phần truy vấn báo cáo vào SQL Server và API chỉ nhận kết quả trả về.

### 12.6. Trình bày thanh toán và transaction

Mở `MiniSupermarket.API/Controllers/OrdersController.cs`, tìm `Checkout`:

> Khi thanh toán, hệ thống khóa và kiểm tra tồn kho, tạo hóa đơn, tạo chi tiết hóa đơn, trừ tồn kho và cập nhật điểm khách hàng. Tất cả thao tác nằm trong cùng một transaction. Nếu thành công thì `CommitAsync`; nếu hết hàng hoặc có lỗi thì `RollbackAsync`, vì vậy không tạo hóa đơn dở dang.

Đây là chức năng chính nên chạy thử `POST /api/Orders/checkout`, sau đó kiểm tra lại đơn hàng và tồn kho trong SQL Server.

### 12.7. Trình bày WinForms

Mở các file theo thứ tự:

1. `FormLogin.cs`: nhận thông tin đăng nhập và gọi API.
2. `SessionManager.cs`: lưu JWT và tự gắn `Bearer token` vào request.
3. `FormMainShell.cs`: hiển thị menu và ẩn/hiện chức năng theo role.
4. `FormPOS.cs`: quét Barcode, thêm sản phẩm vào giỏ, tính tiền và thanh toán.
5. `FormReports.cs`: hiển thị dashboard, doanh thu và sản phẩm bán chạy.

Có thể nói:

> WinForms không truy cập trực tiếp vào cơ sở dữ liệu. Giao diện gửi request qua `ApiClientService` đến API. API xử lý nghiệp vụ rồi trả JSON về để WinForms hiển thị.

### 12.8. Luồng cần nhớ khi được hỏi

```text
WinForms Form
	→ ApiClientService / HttpClient
	→ Controller
	→ DbContext hoặc SqlTransaction
	→ SQL Server
	→ kết quả trả ngược về WinForms
```

Khi giảng viên hỏi một chức năng, hãy trả lời theo ba ý: **request đi từ đâu**, **method nào xử lý**, và **kết quả được trả về hoặc lưu ở đâu**.

### 12.9. Thứ tự demo đề xuất

1. Chạy API và mở Swagger.
2. Login để lấy JWT, nhấn **Authorize**.
3. Hiển thị danh sách Category và Product.
4. Thử tìm kiếm hoặc phân trang sản phẩm.
5. Chạy WinForms và đăng nhập.
6. Mở POS, quét Barcode, thêm sản phẩm vào giỏ và thanh toán.
7. Mở báo cáo doanh thu.
8. Nếu cần chứng minh kỹ thuật, mở `OrdersController.Checkout` để giải thích transaction và rollback.

### 12.10. Giải thích nhanh các từ thường gặp

| Từ trong code | Cách hiểu đơn giản |
|---|---|
| Controller | Nơi nhận và xử lý request từ client |
| DTO | Đối tượng dùng để nhận hoặc trả dữ liệu, không trả trực tiếp entity |
| Entity | Lớp đại diện cho dữ liệu trong cơ sở dữ liệu |
| DbContext | Cầu nối giữa C# và SQL Server thông qua Entity Framework Core |
| JWT | Token chứng minh người dùng đã đăng nhập |
| Role | Quyền của người dùng, ví dụ Admin hoặc Cashier |
| Migration | Lịch sử thay đổi cấu trúc cơ sở dữ liệu |
| Transaction | Nhóm thao tác phải cùng thành công hoặc cùng bị hủy |
