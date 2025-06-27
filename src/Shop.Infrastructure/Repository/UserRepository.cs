namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Shop.Infrastructure.Data;

public class UserRepository:BaseRepository<User>,IUserRepository{

    public UserRepository(AppDbContext context) : base(context) {
    }
}

public interface IUserRepository:IRepository<User> {
    
}