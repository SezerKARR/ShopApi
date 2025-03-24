using Microsoft.AspNetCore.Mvc;
namespace ShopApi.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly IUserService _userService; // Kullanıcı servisi (veritabanı işlemleri için)

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
   
 

    [HttpPost]
    public async Task<IActionResult> GoogleLogin([FromHeader] string Authorization)
    {
        var token = Authorization?.Replace("Bearer ", "");
    
        Console.WriteLine(token);
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            User user = await _userService.GetOrCreateUserAsync(payload.Email, payload.Name);
            return Ok(new { user });
        }
        catch (InvalidJwtException)
        {
            return Unauthorized("Invalid Google token");
        }
    }
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
        issuer: "yourIssuer",
        audience: "yourAudience",
        expires: DateTime.Now.AddHours(1), // 1 saatlik geçerlilik süresi
        signingCredentials: credentials
        );
        TokenRequest tokenRequest = new TokenRequest
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}
public class TokenRequest
{
    public string? Token { get; set; }
}