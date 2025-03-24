namespace ShopApi.Services;

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public interface IUserService
{
    Task<User> GetOrCreateUserAsync(string email, string name);
    Task<User?> GetUserByIdAsync(int id);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetOrCreateUserAsync(string email, string name)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            user = new User { Email = email, Name = name };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        return user;
    }
    public Task<User?> GetUserByIdAsync(int id) {
        var user = _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }
}