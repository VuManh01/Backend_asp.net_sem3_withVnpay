using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project3api_be.Data;
using project3api_be.Models;
using project3api_be.Dtos;
using System.Security.Claims;
using System.Net;
using RestSharp;
using project3api_be.Services;

namespace project3api_be.Controllers
{   
   
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext  _context;
        // private readonly TokenService _tokenService;

        // public AccountController(ApplicationDbContext context, TokenService tokenService)
         public AccountController(ApplicationDbContext context)
        {
            _context = context;
            // _tokenService = tokenService;
        }

        // Tạo tài khoản cho User
        // POST: api/account/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _context.Accounts.AnyAsync(a => a.Email == registerDto.Email))
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Email already exists"
                });
            }

            // Create new account
            var account = new Account
            {
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                FullName = registerDto.FullName,
                RoleId = 2, // Default role ID for regular user
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Registration successful"
            });
        }

        // Tạo tài khoản cho Admin
        // POST: api/account/register-admin
        [AllowAnonymous]
        [HttpPost("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
        {
            // Kiểm tra nếu email đã tồn tại
            if (await _context.Accounts.AnyAsync(a => a.Email == registerDto.Email))
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Email already exists"
                });
            }

            // Tạo tài khoản Admin
            var adminAccount = new Account
            {
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                FullName = registerDto.FullName,
                RoleId = 1, // Role ID cho Admin
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Accounts.Add(adminAccount);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Admin registration successful"
            });
        }
        

        [Authorize(Roles = "Admin,User")]
        // POST: api/account/forgot-password
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {   
            //truy vấn email từ database thông qua _context
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

            if (user == null)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found with this email."
                });
            }
            // tạo link reset password mà không cần token
            var token = Guid.NewGuid().ToString(); // Generate a dummy token
            var resetLink = $"http://localhost:4200/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";
            Console.WriteLine(token);
            var client = new RestClient("https://send.api.mailtrap.io/api/send");
            var request = new RestRequest
            {
                Method = Method.Post,
                RequestFormat = DataFormat.Json,
            };

            request.AddHeader("Authorization", "Bearer 62bf23eda20707516e5423e523d82d6c");
            request.AddJsonBody(new
            {
               from = new {email ="mailstrap@demomailtrap.com"},
               to = new[] {new {email = user.Email}},
               template_uuid = "8703a74f-44cd-4e7a-b243-83fad62a999d",
               template_variables = new {user_email = user.Email, pass_reset_link = resetLink}
            });

            var response = await client.ExecuteAsync(request);
            if(response.IsSuccessful){
                return Ok(new AuthResponseDto{
                    IsSuccess = true,
                    Message = "Email sent with password reset link. Please check your email."
                });
            }else{
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = response.Content!.ToString()
                });
            }
        }


        // POST: api/account/reset-password
        [Authorize(Roles = "Admin,User")]
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);

            if (user == null)
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found with this email."
                });
            }
            // Mã hóa mật khẩu mới trước khi lưu
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            user.UpdatedAt = DateTime.Now;

            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Password reset successfully."
            });
        }

        // [Authorize(Roles = "Admin,User")]
        // [Authorize]
        // GET: api/account/detail
        // [HttpGet("detail/{accountId}")]
        // public async Task<ActionResult<Account>> GetAccountDetail(int accountId)
        // {   
        //     // Lấy ID người dùng trực tiếp từ Claims của token đã được xác thực
        //     // var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     // Console.WriteLine($"Current User ID: {currentUserId}");
        //     // if (string.IsNullOrEmpty(currentUserId))
        //     // {
        //     //     return Unauthorized(new AuthResponseDto
        //     //     {
        //     //         IsSuccess = false,
        //     //         Message = "User is not authenticated."
        //     //     });
        //     // }
        //     // var account = await _context.Accounts
        //     //     .Include(a => a.Role)
        //     //     .FirstOrDefaultAsync(a => a.AccountId.ToString() == currentUserId);

            
        //     var account = await _context.Accounts
        //         .Include(a => a.Role)
        //         .FirstOrDefaultAsync(a => a.AccountId == accountId);

        //     if (account == null)
        //     {
        //         return NotFound(new AuthResponseDto
        //         {
        //             IsSuccess = false,
        //             Message = "Account not found."
        //         });
        //     }

        //     // Loại bỏ thông tin nhạy cảm trước khi trả về
        //     account.Password = null;
            
        //     return Ok(account);
        // }


        // GET: api/account/detail-after-login
        [HttpGet("detail-after-login")]
        public async Task<ActionResult<Account>> GetAccountDetailAfterLogin([FromQuery] int accountId)
        {   
            // Lấy AccountId từ Claims
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new AuthResponseDto
                {
                IsSuccess = false,
                Message = "User is not authenticated."
                });
            }
            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.AccountId.ToString() == currentUserId);

            if (account == null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Account not found."
                });
            }

            // Remove sensitive information before returning
            account.Password = null;

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Account details retrieved successfully",
                Role = account.Role?.RoleName ?? "No Role",
                AccountId = account.AccountId,
                Email = account.Email,
                FullName = account.FullName,
               
            });
        }



        // POST: api/account/logout
        [Authorize(Roles = "Admin,User")]
         // POST: api/account/logout
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            // Clear token or handle logout logic.
            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Logout successful."
            });
        }

        [Authorize(Roles = "Admin,User")]
         // POST: api/account/subscription
        [HttpPost("subscription")]
        public async Task<ActionResult> CreateSubscription([FromBody] SubscriptionDto subscriptionDto)
            {
        if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (currentUserId == null)
            {
                return Unauthorized(new AuthResponseDto
            {
                IsSuccess = false,
                Message = "User is not authenticated."
            });
            }

         // Map từ SubscriptionDto sang Subscription entity
        var subscription = new Subscription
        {
            MembershipServiceId = subscriptionDto.MembershipServiceId,
            Status = subscriptionDto.Status,
            Price = subscriptionDto.Price,
            CreatedAt = subscriptionDto.CreatedAt?? DateTime.UtcNow,
            UpdatedAt = subscriptionDto.UpdatedAt?? DateTime.UtcNow
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Subscription created successfully.",
            Token = subscription.SubId.ToString()
        });
    }

    }
}