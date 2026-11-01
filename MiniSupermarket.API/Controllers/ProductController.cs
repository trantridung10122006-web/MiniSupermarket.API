using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public ProductsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. GET: Lấy danh sách sản phẩm (hỗ trợ phân trang Skip/Take kết hợp .AsNoTracking() - TC25)
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] int? skip,
            [FromQuery] int? take)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            int totalCount = await query.CountAsync();

            // Nếu truyền skip / take trực tiếp
            if (skip.HasValue || take.HasValue)
            {
                int s = Math.Max(0, skip ?? 0);
                int t = take.HasValue && take.Value > 0 ? take.Value : 10;

                Response.Headers["X-Total-Count"] = totalCount.ToString();
                Response.Headers["X-Skip"] = s.ToString();
                Response.Headers["X-Take"] = t.ToString();

                var pagedBySkipTake = await query
                    .OrderBy(p => p.ProductId)
                    .Skip(s)
                    .Take(t)
                    .ToListAsync();

                return Ok(pagedBySkipTake.Select(ProductDto.FromEntity));
            }

            // Nếu truyền pageNumber / pageSize
            if (pageNumber.HasValue || pageSize.HasValue)
            {
                int page = Math.Max(1, pageNumber ?? 1);
                int size = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 10;
                int totalPages = (int)Math.Ceiling((double)totalCount / size);

                Response.Headers["X-Total-Count"] = totalCount.ToString();
                Response.Headers["X-Page-Number"] = page.ToString();
                Response.Headers["X-Page-Size"] = size.ToString();
                Response.Headers["X-Total-Pages"] = totalPages.ToString();

                var pagedList = await query
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToListAsync();

                return Ok(pagedList.Select(ProductDto.FromEntity));
            }

            // Mặc định nạp toàn bộ danh mục cho WinForms Client nếu không yêu cầu phân trang
            var list = await query.OrderBy(p => p.ProductId).ToListAsync();
            Response.Headers["X-Total-Count"] = totalCount.ToString();
            return Ok(list.Select(ProductDto.FromEntity));
        }

        // 1.1 GET: Endpoint chuyên dụng trả về DTO phân trang đầy đủ thông tin metadata
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            int page = Math.Max(1, pageNumber);
            int size = Math.Max(1, pageSize);

            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            var items = await query
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return Ok(new
            {
                totalItems = totalItems,
                pageNumber = page,
                pageSize = size,
                totalPages = totalPages,
                items = items.Select(ProductDto.FromEntity)
            });
        }

        // 2. GET: Lấy chi tiết sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm!" });
            }
            return Ok(ProductDto.FromEntity(product));
        }

        // 3. GET: Tra cứu sản phẩm theo mã vạch (Barcode)
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Barcode == barcode);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm với mã vạch này!" });
            }
            return Ok(ProductDto.FromEntity(product));
        }

        // 4. GET: Tìm kiếm theo tên sản phẩm
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });
            }

            var result = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => (p.ProductName != null && p.ProductName.Contains(keyword)) ||
                            (p.Barcode != null && p.Barcode.Contains(keyword)))
                .ToListAsync();

            return Ok(result.Select(ProductDto.FromEntity));
        }

        // 5. GET: Lọc sản phẩm theo danh mục
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var result = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            return Ok(result.Select(ProductDto.FromEntity));
        }

        // 6. POST: Thêm mới sản phẩm
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == request.CategoryId);
            if (!categoryExists)
            {
                return BadRequest(new { message = "Danh mục không tồn tại!" });
            }

            var newProduct = new Product
            {
                Barcode = request.Barcode,
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
                SupplierId = request.SupplierId
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, ProductDto.FromEntity(newProduct));
        }

        // 7. PUT: Cập nhật sản phẩm
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto updateProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần sửa!" });
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == updateProduct.CategoryId);
            if (!categoryExists)
            {
                return BadRequest(new { message = "Danh mục không tồn tại!" });
            }

            product.Barcode = updateProduct.Barcode;
            product.ProductName = updateProduct.ProductName;
            product.Price = updateProduct.Price;
            product.StockQuantity = updateProduct.StockQuantity;
            product.CategoryId = updateProduct.CategoryId;
            product.BrandId = updateProduct.BrandId;
            product.SupplierId = updateProduct.SupplierId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 8. DELETE: Xóa sản phẩm
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần xóa!" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
