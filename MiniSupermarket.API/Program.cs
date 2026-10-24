using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using MiniSupermarket.API.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// JWT Authentication
// =========================

var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!";

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)
            ),

            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// =========================
// Controllers
// =========================

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

// =========================
// Database
// =========================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SupermarketDbContext>(options =>
    options.UseSqlServer(connectionString));

// =========================
// Swagger
// =========================

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mini Supermarket API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT Token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =========================
// Build
// =========================

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SupermarketDbContext>();

    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseInitialization");
        logger.LogError(ex, "Không thể tạo hoặc cập nhật database. Kiểm tra chuỗi kết nối SQL Server.");
        throw;
    }
}

// =========================
// Middleware
// =========================

var hasHttpsPort = !string.IsNullOrWhiteSpace(
    builder.Configuration["ASPNETCORE_HTTPS_PORT"])
    || (builder.Configuration["ASPNETCORE_URLS"] ?? string.Empty)
        .Contains("https://", StringComparison.OrdinalIgnoreCase);

if (hasHttpsPort && !app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Luôn bật Swagger để kiểm tra API tiện lợi
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini Supermarket API v1");
});

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// Swagger UI nên mở đúng cả khi truy cập /swagger hoặc /
app.MapGet("/", () => Results.Redirect("/swagger/"));
app.MapGet("/swagger", () => Results.Redirect("/swagger/"));

app.MapControllers();

// ========================================================
// Tự động bật Chrome mở giao diện Swagger khi khởi động API
// ========================================================
if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        try
        {
            string swaggerUrl = "http://localhost:5000/swagger";

            // Tìm file thực thi Google Chrome trên máy Windows
            string[] chromePaths = new[]
            {
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\Application\chrome.exe")
            };

            string? chromeExe = chromePaths.FirstOrDefault(File.Exists);

            if (chromeExe != null)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = chromeExe,
                    Arguments = swaggerUrl,
                    UseShellExecute = false
                });
                return;
            }

            // Gọi thử qua biến môi trường PATH
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "chrome.exe",
                    Arguments = swaggerUrl,
                    UseShellExecute = true
                });
                return;
            }
            catch
            {
            }

            // Fallback mở trình duyệt mặc định nếu không thấy Chrome
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = swaggerUrl,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILoggerFactory>()
                .CreateLogger("SwaggerLauncher");
            logger.LogWarning(ex, "Không thể tự động mở trình duyệt Swagger.");
        }
    });
}

app.Run();
