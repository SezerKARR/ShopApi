namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;
public interface IBasketRepository:IRepository<Basket> {
    Task<Basket?> GetBasketByUserIpAsync(string userIp);
    Task<Basket?> GetBasketByUserIdAsync(string? userId);
}
public class BasketRepository:BaseRepository<Basket>,IBasketRepository {

    public BasketRepository(AppDbContext context) : base(context) {
    }
    public async Task<Basket?> GetBasketByUserIpAsync(string userIp) {
        Basket? basket= await Queryable.FirstOrDefaultAsync(basket => basket.UserIp == userIp);
        return basket;
    }
    public async Task<Basket?> GetBasketByUserIdAsync(string? userId) {
        Basket? basket= await Queryable.FirstOrDefaultAsync(basket => basket.UserId == userId);
        return basket;
    }
}

