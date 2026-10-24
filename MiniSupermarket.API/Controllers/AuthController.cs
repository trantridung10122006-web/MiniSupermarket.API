using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniSupermarket.API.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SupermarketDbContext _context;

        public AuthController(IConfiguration configuration, SupermarketDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // Endpoint Đăng nhập: POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // 1. Kiểm tra tài khoản trong cơ sở dữ liệu
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower() && u.PasswordHash == request.Password);

            if (user != null)
            {
                string roleName = user.Role?.RoleName ?? "Cashier";
                if (roleName.Equals("Thu Ngân", StringComparison.OrdinalIgnoreCase)) roleName = "Cashier";
                if (roleName.Equals("Khách Hàng", StringComparison.OrdinalIgnoreCase)) roleName = "Customer";

                var linkedCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.FullName == user.FullName || c.PhoneNumber == user.Username);

                var token = GenerateJwtToken(user.Username, roleName);
                return Ok(new
                {
                    success = true,
                    token = token,
                    role = roleName,
                    username = user.Username,
                    fullName = user.FullName,
                    customerId = linkedCustomer?.CustomerId,
                    customerPhone = linkedCustomer?.PhoneNumber
                });
            }

            // 1.5 Cho phép khách hàng đăng nhập trực tiếp bằng Số điện thoại (mật khẩu 123456 hoặc chính SĐT)
            var cust = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == request.Username.Trim());
            if (cust != null && (request.Password == "123456" || request.Password == cust.PhoneNumber))
            {
                var token = GenerateJwtToken(cust.PhoneNumber ?? request.Username, "Customer");
                return Ok(new
                {
                    success = true,
                    token = token,
                    role = "Customer",
                    username = cust.PhoneNumber,
                    fullName = cust.FullName,
                    customerId = cust.CustomerId,
                    customerPhone = cust.PhoneNumber
                });
            }

            // 2. Kiểm tra tài khoản mẫu / fallback
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = GenerateJwtToken(request.Username, "Admin");
                return Ok(new { success = true, token = token, role = "Admin" });
            }
            else if (request.Username == "cashier" && request.Password == "123456")
            {
                var token = GenerateJwtToken(request.Username, "Cashier");
                return Ok(new { success = true, token = token, role = "Cashier" });
            }

            return Unauthorized(new { success = false, message = "Sai tài khoản hoặc mật khẩu!" });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Lấy khóa bí mật từ appsettings.json
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Thời hạn token là 2 tiếng
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
