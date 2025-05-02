namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public interface IBasketRepository:IRepository<Basket> {
    Task<Basket?> GetBasketByUserIpAsync(string userIp,int includes=-1);
    Task<Basket?> GetBasketByUserIdAsync(string? userId,int includes=-1);
}
public class BasketRepository:BaseRepository<Basket>,IBasketRepository {

    public BasketRepository(AppDbContext context) : base(context) {
    }
    public async Task<Basket?> GetBasketByUserIpAsync(string userIp,int includes=-1) {
        Basket? basket= await IncludeQuery(includes).FirstOrDefaultAsync(basket => basket.UserIp == userIp);
        return basket;
    }
    public async Task<Basket?> GetBasketByUserIdAsync(string? userId,int includes=-1) {
        Basket? basket= await IncludeQuery(includes).FirstOrDefaultAsync(basket => basket.UserId == userId);
        return basket;
    }
}

