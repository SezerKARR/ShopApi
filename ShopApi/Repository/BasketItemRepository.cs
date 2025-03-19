namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;
public interface IBasketItemRepository: IRepository<BasketItem> {
    Task<List<BasketItem>> GetBasketItemsByBasketId(int basketId);
}
public class BasketItemRepository:BaseRepository<BasketItem>,IBasketItemRepository  {

    public BasketItemRepository(AppDbContext context) : base(context) {
    }
    
    public async Task<List<BasketItem>> GetBasketItemsByBasketId(int basketId) {
        List<BasketItem> basketItems = await Queryable.Where(basketItem => basketItem.BasketId == basketId).ToListAsync();
        return basketItems;
    }
}

