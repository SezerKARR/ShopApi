namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public interface IBasketItemRepository: IRepository<BasketItem> {
    Task<List<BasketItem>> GetBasketItemsByBasketId(int basketId);
}
public class BasketItemRepository:BaseRepository<BasketItem>,IBasketItemRepository  {

    public BasketItemRepository(AppDbContext context) : base(context) {
    }
    
    public async Task<List<BasketItem>> GetBasketItemsByBasketId(int basketId) {
        List<BasketItem> basketItems = await IncludeQuery(-1).Where(basketItem => basketItem.BasketId == basketId).ToListAsync();
        return basketItems;
    }
}

