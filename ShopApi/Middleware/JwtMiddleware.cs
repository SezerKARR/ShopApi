namespace ShopApi.Middleware;

using System.IdentityModel.Tokens.Jwt;
using Models;
using Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly IUserService _userService; // Kullanıcı servisini kullanacağız

    public JwtMiddleware(RequestDelegate next, IConfiguration config, IUserService userService)
    {
        _next = next;
        _config = config;
        _userService = userService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var user = ValidateToken(token);
            context.Items["User"] = user;
        }
        await _next(context); 
    }

    private async Task<User?> ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (int.TryParse(userIdClaim, out var userId))
        {
            return await _userService.GetUserByIdAsync(userId);
        }
    
        return null;
    }
}