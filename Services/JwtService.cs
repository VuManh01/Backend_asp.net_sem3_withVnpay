// namespace project3api_be.Services;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using project3api_be.Models;
// public class JwtService
// {
//     private readonly string _secretKey;
//     private readonly string _issuer;
//     private readonly string _audience;

//     public JwtService(IConfiguration configuration)
//     {
//         _secretKey = configuration["Jwt:SecretKey"];
//         _issuer = configuration["Jwt:ValidIssuer"];
//         _audience = configuration["Jwt:ValidAudience"];
//     }

//     public string GenerateToken(Account account)
//     {

//         if (account == null)
//             throw new ArgumentNullException(nameof(account));
//         if (account.Role == null)
//             throw new ArgumentException("Account role is not loaded or null");
// Console.WriteLine($"Generating token for AccountId: {account.AccountId}");
// Console.WriteLine("====================");

//         var claims = new[]
//         {   
//             new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
//             new Claim(ClaimTypes.Name, account.FullName ?? ""),
//             new Claim(ClaimTypes.Email, account.Email ?? ""),
//             new Claim(ClaimTypes.Role, account.Role.RoleName ?? "") // Lấy Role từ bảng Roles
//         };

// // In ra log để kiểm tra claims
// foreach (var claim in claims)
//    {
//        Console.WriteLine($"Claim added - Type: {claim.Type}, Value: {claim.Value}");
//    }

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//         var token = new JwtSecurityToken(
//             issuer: _issuer,
//             audience: _audience,
//             claims: claims,
//             expires: DateTime.Now.AddDays(1),
//             signingCredentials: creds
//         );

//         // return new JwtSecurityTokenHandler().WriteToken(token);
//         var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
//          Console.WriteLine("== TOKEN GENERATION ==");
//             Console.WriteLine($"SecretKey used for generation: {_secretKey}");

//        Console.WriteLine($"Issuer: {_issuer}");
//        Console.WriteLine($"Audience: {_audience}");
//        Console.WriteLine($"Token: {tokenString}");
//         Console.WriteLine("====================");

//       // Kiểm tra format token
//        var parts = tokenString.Split('.');
//        if (parts.Length != 3)
//        {
//            throw new Exception($"Invalid token format. Expected 3 parts, got {parts.Length}");
//        }  

//           // Thêm bước kiểm tra xem token có hợp lệ không
//     var handler = new JwtSecurityTokenHandler();
//     try
//     {
//         var jwtToken = handler.ReadToken(tokenString) as JwtSecurityToken;
//         if (jwtToken == null)
//         {
//             throw new Exception("Invalid token format.");
//         }
//         Console.WriteLine("Token is valid.");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error reading token: {ex.Message}");
//     }  


//    return tokenString;
//     }
    
// }


