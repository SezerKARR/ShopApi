namespace ShopApi.Services;

using Data;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Models;
using Repository;

public interface IUserService
{
    Task<User> GetOrCreateUserAsync(string email, string name);
    Task<User?> GetUserByIdAsync(int id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    readonly IUnitOfWork _unitOfWork;

    public UserService( IUserRepository userRepository,IUnitOfWork unitOfWork) {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> GetOrCreateUserAsync(string email, string name)
    {
        var user = await _userRepository.GetBySlugAsync(SlugHelper.GenerateSlug(name));
        if (user == null)
        {
            user = new User { Email = email, Name = name };
            await _userRepository.CreateAsync(user);
            await _unitOfWork.CommitAsync();
        }
        return user;
    }
    public Task<User?> GetUserByIdAsync(int id) {
        var user = _userRepository.GetByIdAsync(id);
        return user;
    }
}