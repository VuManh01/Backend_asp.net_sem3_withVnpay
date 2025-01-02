using project3api_be.Data;
using project3api_be.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BCrypt.Net;
using System.Text;
using project3api_be.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var JWTSetting = builder.Configuration.GetSection("Jwt");

// Cấu hình DbContext sử dụng MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 403))));

// builder.Services.AddScoped<JwtService>();
builder.Services.AddSingleton<TokenService>();


// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {   
//         // Bỏ events cũ, thêm events mới
//         options.Events = new JwtBearerEvents
//         {
//             OnMessageReceived = context =>
//             {
//                 var authHeader = context.Request.Headers["Authorization"].ToString();
//                 Console.WriteLine($"Auth header received: {authHeader}");
//                 Console.WriteLine("====================");
                
//                 if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//                 {
//                     // Chỉ lấy phần token, loại bỏ "Bearer "
//                     var token = authHeader.Substring("Bearer ".Length).Split()[0]; // Lấy phần đầu tiên sau Bearer
//                     Console.WriteLine($"Clean token: {token}");
//                     context.Token = token;
//                 }
                
//                 return Task.CompletedTask;
//             },
//              // Thêm các event handler này
//            OnTokenValidated = context =>
//            {
//                Console.WriteLine("Token đã được xác thực thành công!");
//                var identity = context.Principal?.Identity as ClaimsIdentity;
//                if (identity != null)
//                {
//                    foreach (var claim in identity.Claims)
//                    {
//                        Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
//                    }
//                }
//                return Task.CompletedTask;
//            },
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine($"Xác thực thất bại!");
//                Console.WriteLine($"Loại lỗi: {context.Exception.GetType().Name}");
//                Console.WriteLine($"Message: {context.Exception.Message}");
//                Console.WriteLine($"StackTrace: {context.Exception.StackTrace}");
//                return Task.CompletedTask;
//            },
//            OnChallenge = context =>
//            {
//                Console.WriteLine("Token bị từ chối!");
//                Console.WriteLine($"Error: {context.Error}");
//                Console.WriteLine($"Error Description: {context.ErrorDescription}");
//                return Task.CompletedTask;
//            }

//         };

//         // Sửa lại TokenValidationParameters
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
//             ValidAudience = builder.Configuration["Jwt:ValidAudience"],
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])
//             ),
//             RequireSignedTokens = true,
//             RequireExpirationTime = true,
//             ClockSkew = TimeSpan.Zero  // Thêm dòng này để xử lý chính xác thời gian
//         };
        
//         // Bỏ qua SSL validation nếu đang phát triển
//         options.RequireHttpsMetadata = false;
//     });


// // Cấu hình JWT Authentication
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {   
//     var secretKey = builder.Configuration["Jwt:SecretKey"];
//         Console.WriteLine("== TOKEN VALIDATION ==");
//        Console.WriteLine($"SecretKey used for validation: {secretKey}");

//     options.SaveToken = true; // Thêm dòng này
//     options.RequireHttpsMetadata = false; // Trong môi trường dev
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
//         ValidAudience = builder.Configuration["Jwt:ValidAudience"],
//         IssuerSigningKey = new SymmetricSecurityKey(
//             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])
//         ),
//         ClockSkew = TimeSpan.Zero
//     };

//     options.Events = new JwtBearerEvents
//     {
//         OnMessageReceived = context =>
//         {   
//             Console.WriteLine("====================");
//    Console.WriteLine("== VALIDATION PARAMETERS");
//    Console.WriteLine($"ValidIssuer: {options.TokenValidationParameters.ValidIssuer}");
//    Console.WriteLine($"ValidAudience: {options.TokenValidationParameters.ValidAudience}");
//    Console.WriteLine($"SecretKey length: {builder.Configuration["Jwt:SecretKey"]?.Length}");
//           Console.WriteLine($"SecretKey used for validation: {secretKey}");

//             Console.WriteLine("====================");
//             Console.WriteLine("== BEFRORE VALIDATE TOKEN");
//             var authHeader = context.Request.Headers["Authorization"].ToString();
//             Console.WriteLine($"token before validate: {authHeader}");
           
//             if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//             {
//                 var token = authHeader["Bearer ".Length..].Trim();
//                 Console.WriteLine($"Clean token: {token}");

//                 // Kiểm tra format token
//            if (token.Count(c => c == '.') == 2) // JWT phải có 2 dấu chấm
//            {
//                context.Token = token;
//                Console.WriteLine("Token format is valid");
//            }
//            else
//            {
//                Console.WriteLine("Invalid token format - dots count: " + token.Count(c => c == '.'));
//            }
//                 // Thêm vào OnMessageReceived để debug
// var tokenParts = token.Split('.');
// Console.WriteLine($"Token parts count: {tokenParts.Length}");
// foreach (var part in tokenParts)
// {
//     Console.WriteLine($"Part length: {part.Length}");
// }
//                 // context.Token = token;
//                 Console.WriteLine("Token format is valid and set to context");
//             }else
//        {
//            Console.WriteLine("Invalid token format or no token provided");
//        }
//             return Task.CompletedTask;
//         },


//         OnTokenValidated = context =>
//         {
//             // Console.WriteLine("====================");
//             // Console.WriteLine("== AFTER VALIDATE TOKEN");
//             // Console.WriteLine($"Token after validation: {context.SecurityToken}");
//             // Console.WriteLine("Token validated successfully");
//             // var identity = context.Principal?.Identity as ClaimsIdentity;
//             // if (identity != null)
//             // {
//             //     foreach (var claim in identity.Claims)
//             //     {
//             //         Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
//             //     }
//             // }
//             // return Task.CompletedTask;
//             Console.WriteLine("====================");
//                Console.WriteLine("== TOKEN VALIDATED");
//                var token = context.SecurityToken as JwtSecurityToken;
//                Console.WriteLine($"Issuer: {token?.Issuer}");
//                Console.WriteLine($"Audience: {token?.Audiences.FirstOrDefault()}");
//                Console.WriteLine($"Valid from: {token?.ValidFrom}");
//                Console.WriteLine($"Valid to: {token?.ValidTo}");
//                return Task.CompletedTask;
//         },

//         OnChallenge = context =>
//    {
//        Console.WriteLine("====================");
//        Console.WriteLine("== TOKEN CHALLENGE");
//        Console.WriteLine($"Error: {context.Error}");
//        Console.WriteLine($"Error Description: {context.ErrorDescription}");
//        return Task.CompletedTask;
//    },

//          OnAuthenticationFailed = context =>
//    {
//        Console.WriteLine("====================");
//        Console.WriteLine("== AUTHENTICATION FAILED");
//        Console.WriteLine($"Exception Type: {context.Exception.GetType().Name}");
//        Console.WriteLine($"Message: {context.Exception.Message}");
       
//        if (context.Exception is SecurityTokenExpiredException)
//        {
//            Console.WriteLine("Token is expired");
//        }
//        else if (context.Exception is SecurityTokenInvalidIssuerException)
//        {
//            Console.WriteLine("Invalid issuer");
//        }
//        else if (context.Exception is SecurityTokenInvalidAudienceException)
//        {
//            Console.WriteLine("Invalid audience");
//        }
       
//        return Task.CompletedTask;
//    }
//     };
// });



builder.Services.AddControllers();

// Cấu hình Cookie cho Authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login"; // Đường dẫn đăng nhập
        options.AccessDeniedPath = "/forbidden"; // Đường dẫn nếu bị từ chối quyền
    });


// Cấu hình Authorization với Roles
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminPolicy", policy =>
//         policy.RequireRole("Admin")); // Thêm chính sách phân quyền cho "Admin"
// });


// Cấu hình Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization Example: 'Bearer elelelelelleleleelelelele'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddSingleton<IVnPayService, VnPayService>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
});

// Thêm logging
// app.Use(async (context, next) =>
// {
//     var tokenService = context.RequestServices.GetRequiredService<TokenService>();
//     var authHeader = context.Request.Headers["Authorization"].ToString();
//     if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//    {
//        // Chỉ lưu phần token, không lưu "Bearer "
//        tokenService.CurrentToken = authHeader.Split()[1]; // Lấy phần sau "Bearer "
//        Console.WriteLine($"Stored token: {tokenService.CurrentToken}");
//    }
//     await next();
// });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();