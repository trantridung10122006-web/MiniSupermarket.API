using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public CustomersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. GET: Lấy toàn bộ danh sách khách hàng
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Customers
                .AsNoTracking()
                .OrderByDescending(c => c.CustomerId)
                .ToListAsync();

            return Ok(list.Select(CustomerDto.FromEntity));
        }

        // 1.5 GET: Bảng xếp hạng điểm tích lũy khách hàng
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var list = await _context.Customers
                .AsNoTracking()
                .OrderByDescending(c => c.RewardPoints ?? 0)
                .ThenBy(c => c.FullName)
                .ToListAsync();

            int rank = 1;
            var result = list.Select(c => new
            {
                Rank = rank++,
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber ?? string.Empty,
                MaskedPhone = string.IsNullOrWhiteSpace(c.PhoneNumber) || c.PhoneNumber.Length < 7
                    ? (c.PhoneNumber ?? "")
                    : c.PhoneNumber.Substring(0, 3) + "****" + c.PhoneNumber.Substring(c.PhoneNumber.Length - 3),
                Points = c.RewardPoints ?? 0,
                MembershipRank = string.IsNullOrEmpty(c.MembershipRank) ? "Đồng" : c.MembershipRank,
                CreatedAt = c.CreatedAt
            }).ToList();

            return Ok(result);
        }

        // 2. GET: Lấy chi tiết khách hàng theo ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng!" });
            }
            return Ok(CustomerDto.FromEntity(customer));
        }

        // 3. GET: Tìm kiếm khách hàng theo số điện thoại hoặc tên (Dùng cho màn hình POS)
        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest(new { message = "Từ khóa tìm kiếm không hợp lệ!" });
            }

            string clean = phone.Trim();
            
            // 1. Tìm chính xác theo số điện thoại
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PhoneNumber == clean);

            // 2. Tìm theo tên khách hàng nếu không thấy theo SĐT
            if (customer == null)
            {
                customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.FullName != null && c.FullName.ToLower() == clean.ToLower());
            }

            if (customer == null)
            {
                customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.FullName != null && c.FullName.Contains(clean));
            }

            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng trong hệ thống!" });
            }

            return Ok(CustomerDto.FromEntity(customer));
        }

        // 4. GET: Tìm kiếm theo tên hoặc số điện thoại
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });
            }

            string term = keyword.Trim().ToLower();
            var result = await _context.Customers
                .AsNoTracking()
                .Where(c => (c.FullName != null && c.FullName.ToLower().Contains(term)) ||
                            (c.PhoneNumber != null && c.PhoneNumber.Contains(term)) ||
                            (c.Email != null && c.Email.ToLower().Contains(term)))
                .ToListAsync();

            return Ok(result.Select(CustomerDto.FromEntity));
        }

        // 5. GET: Lấy lịch sử đơn hàng của khách hàng
        [HttpGet("{id:int}/orders")]
        public async Task<IActionResult> GetCustomerOrders(int id)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.CustomerId == id)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderHistoryDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CashierUsername = o.User != null ? o.User.Username : "—",
                    ItemCount = o.OrderItems != null ? o.OrderItems.Sum(oi => oi.Quantity) : 0,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return Ok(orders);
        }

        // 6. POST: Thêm mới khách hàng
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? phone = request.GetPhoneNumber();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                bool exists = await _context.Customers.AnyAsync(c => c.PhoneNumber == phone);
                if (exists)
                {
                    return Conflict(new { message = "Số điện thoại này đã được sử dụng cho một khách hàng khác!" });
                }
            }

            var newCustomer = new Customer
            {
                FullName = request.FullName.Trim(),
                PhoneNumber = phone,
                Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
                Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
                RewardPoints = request.GetRewardPoints(),
                MembershipRank = string.IsNullOrWhiteSpace(request.MembershipRank) ? "Đồng" : request.MembershipRank.Trim(),
                CreatedAt = DateTime.Now
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newCustomer.CustomerId }, CustomerDto.FromEntity(newCustomer));
        }

        // 7. PUT: Cập nhật thông tin khách hàng
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng cần sửa!" });
            }

            string? phone = request.GetPhoneNumber();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                bool duplicate = await _context.Customers.AnyAsync(c => c.PhoneNumber == phone && c.CustomerId != id);
                if (duplicate)
                {
                    return Conflict(new { message = "Số điện thoại này đã thuộc về khách hàng khác!" });
                }
            }

            customer.FullName = request.FullName.Trim();
            customer.PhoneNumber = phone;
            customer.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
            customer.Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim();
            customer.RewardPoints = request.GetRewardPoints();
            if (!string.IsNullOrWhiteSpace(request.MembershipRank))
            {
                customer.MembershipRank = request.MembershipRank.Trim();
            }

            await _context.SaveChangesAsync();
            return Ok(CustomerDto.FromEntity(customer));
        }

        // 8. DELETE: Xóa khách hàng
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Không tìm thấy khách hàng cần xóa!" });
            }

            bool hasOrders = await _context.Orders.AnyAsync(o => o.CustomerId == id);
            if (hasOrders)
            {
                return BadRequest(new { message = "Không thể xóa khách hàng đã có lịch sử đơn hàng trong hệ thống!" });
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
