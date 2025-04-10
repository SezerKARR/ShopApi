namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class UserRepository:BaseRepository<User>,IUserRepository{

    public UserRepository(AppDbContext context) : base(context) {
    }
}

public interface IUserRepository:IRepository<User> {
    
}