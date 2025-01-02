using project3api_be.Data;
using project3api_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using project3api_be.Services;
using project3api_be.Dtos;
namespace project3api_be.Controllers;
using Microsoft.AspNetCore.Authentication;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{   
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    // private readonly JwtService _jwtService;

    // public AuthController(ApplicationDbContext context, JwtService jwtService,IConfiguration configuration)
    public AuthController(ApplicationDbContext context, IConfiguration configuration)

    {
        _configuration = configuration;
        _context = context;
        // _jwtService = jwtService;
    }


    // API đăng nhập

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Kiểm tra đầu vào
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Email and password are required");
        }
        // Tìm tài khoản dựa trên email
        var account = await _context.Accounts
       .Include(a => a.Role)  // Thêm dòng này để load Role
       .FirstOrDefaultAsync(a => a.Email == request.Email);
        if (account == null)
        {
            return Unauthorized("Invalid credentials");
        }
        // So sánh mật khẩu đã mã hóa
        if (!BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        // tạo Claims
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, account.Role?.RoleName ?? "No Role")
        };
         var claimsIdentity = new ClaimsIdentity(claims, "login");

        // Tạo ClaimsPrincipal
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Đăng nhập người dùng
        await HttpContext.SignInAsync("Cookies",claimsPrincipal);

        // Tạo JWT token nếu mật khẩu chính xác
        // string jwt = _jwtService.GenerateToken(account);

        // Set cookie with JWT token
        // Response.Cookies.Append("jwt", jwt, new CookieOptions
        // {
        //     HttpOnly = true,
        //     SameSite = SameSiteMode.Strict,
        //     Secure = true
        // });

        return Ok(new AuthResponseDto{ 
            // Token = jwt,
            IsSuccess = true,  
            Message = "Login successful", 
            Role = account.Role?.RoleName ?? "No Role",
            AccountId = account.AccountId,
            Email = account.Email,
            FullName = account.FullName,
            
            // User = new
            // {
            //     account.FullName,
            //     account.Email,
            //     RoleName = account.Role?.RoleName ?? "No Role"
            // } 
        });
    }
}