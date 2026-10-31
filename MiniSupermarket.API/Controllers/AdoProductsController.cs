using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using System.Data;

namespace MiniSupermarket.API.Controllers
{
    // Controller thuần ADO.NET tối ưu hóa tốc độ tra cứu mã vạch POS dưới 5ms (TC32, TC33)
    [Authorize]
    [Route("api/products/barcode-ado")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdoProductsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public AdoProductsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // GET: api/products/barcode-ado/{barcode} hoặc api/adoproducts/{barcode}
        [HttpGet("{barcode}")]
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(new { message = "Mã vạch không được để trống!" });
            }

            var connectionString = _context.Database.GetConnectionString();

            // 1. Sử dụng kết nối trực tiếp SqlConnection
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // 2. Sử dụng SqlCommand với truy vấn có tham số hóa (Parameterized Query - TC33 chống SQL Injection)
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    p.ProductId, 
                    p.Barcode, 
                    p.ProductName, 
                    p.Price, 
                    p.StockQuantity, 
                    p.CategoryId, 
                    c.CategoryName, 
                    p.BrandId, 
                    p.SupplierId
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
                WHERE p.Barcode = @barcode";

            command.Parameters.AddWithValue("@barcode", barcode.Trim());

            // 3. Sử dụng luồng đọc siêu tốc SqlDataReader (TC32)
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var product = new
                {
                    productId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    barcode = reader.IsDBNull(reader.GetOrdinal("Barcode")) ? null : reader.GetString(reader.GetOrdinal("Barcode")),
                    productName = reader.GetString(reader.GetOrdinal("ProductName")),
                    price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    stockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                    categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    categoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString(reader.GetOrdinal("CategoryName")),
                    brandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("BrandId")),
                    supplierId = reader.IsDBNull(reader.GetOrdinal("SupplierId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SupplierId"))
                };

                return Ok(product);
            }

            return NotFound(new { message = "Không tìm thấy sản phẩm với mã vạch này (ADO.NET)!" });
        }
    }
}
