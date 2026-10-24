using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public UsersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/users (Lấy danh sách người dùng)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .Select(u => new UserDto
                {
                    Id = u.UserId,
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    RoleId = u.RoleId,
                    Role = u.Role != null ? u.Role.RoleName : "Cashier",
                    IsActive = true
                })
                .ToListAsync();

            return Ok(users);
        }

        // 2. GET: api/users/{id} (Chi tiết người dùng)
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    Id = u.UserId,
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    RoleId = u.RoleId,
                    Role = u.Role != null ? u.Role.RoleName : "Cashier",
                    IsActive = true
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            return Ok(user);
        }

        // 3. POST: api/users (Tạo tài khoản người dùng mới)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string username = (request.Username ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new { message = "Tên đăng nhập không được để trống!" });
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Mật khẩu không được để trống!" });
            }

            // Kiểm tra trùng username
            bool exists = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());

            if (exists)
            {
                return Conflict(new { message = "Tên đăng nhập đã tồn tại!" });
            }

            // Xác định vai trò (Role)
            Role? role = null;
            string requestedRole = (request.Role ?? string.Empty).Trim();

            if (request.RoleId.HasValue && request.RoleId.Value > 0)
            {
                role = await _context.Roles.FindAsync(request.RoleId.Value);
            }

            if (role == null && !string.IsNullOrEmpty(requestedRole))
            {
                role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName.ToLower() == requestedRole.ToLower());

                if (role == null)
                {
                    if (requestedRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
                    {
                        role = await _context.Roles
                            .FirstOrDefaultAsync(r => r.RoleName == "Thu Ngân" || r.RoleId == 2);
                    }
                    else if (requestedRole.Equals("Warehouse", StringComparison.OrdinalIgnoreCase) ||
                             requestedRole.Equals("Kho", StringComparison.OrdinalIgnoreCase))
                    {
                        role = await _context.Roles
                            .FirstOrDefaultAsync(r => r.RoleName == "Kho" || r.RoleName == "Thủ Kho" || r.RoleName == "Warehouse");

                        if (role == null)
                        {
                            role = new Role
                            {
                                RoleName = "Warehouse",
                                Description = "Nhân viên quản lý kho hàng"
                            };
                            _context.Roles.Add(role);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            if (role == null)
            {
                role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 2)
                    ?? await _context.Roles.FirstAsync();
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = request.Password,
                FullName = (request.FullName ?? string.Empty).Trim(),
                RoleId = role.RoleId
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var resultDto = new UserDto
            {
                Id = newUser.UserId,
                UserId = newUser.UserId,
                Username = newUser.Username,
                FullName = newUser.FullName,
                RoleId = newUser.RoleId,
                Role = role.RoleName,
                IsActive = true
            };

            return CreatedAtAction(nameof(GetById), new { id = newUser.UserId }, resultDto);
        }

        // 4. DELETE: api/users/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 1)
            {
                return BadRequest(new { message = "Không thể xóa tài khoản Quản trị viên mặc định!" });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng cần xóa!" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống!")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FullName { get; set; }

        public string? Role { get; set; }
        public int? RoleId { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
