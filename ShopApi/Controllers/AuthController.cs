namespace ShopApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser([FromRoute]int id) {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> GoogleLogin([FromHeader] string authorization)
    {
        var token = authorization.Replace("Bearer ", "");
    
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
    // private string GenerateJwtToken(User user)
    // {
    //     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"));
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //
    //     var token = new JwtSecurityToken(
    //     issuer: "yourIssuer",
    //     audience: "yourAudience",
    //     expires: DateTime.Now.AddHours(1), // 1 saatlik geçerlilik süresi
    //     signingCredentials: credentials
    //     );
    //     TokenRequest tokenRequest = new TokenRequest
    //     {
    //         Token = new JwtSecurityTokenHandler().WriteToken(token)
    //     };
    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }
    //
}
public class TokenRequest
{
    public string? Token { get; set; }
}