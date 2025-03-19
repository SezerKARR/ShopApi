namespace ShopApi.Repository;

using Data;
using Interface;
using Models;
public interface IBasketRepository:IRepository<Basket> {
}
public class BasketRepository:BaseRepository<Basket>,IBasketRepository {

    public BasketRepository(AppDbContext context) : base(context) {
    }
}

