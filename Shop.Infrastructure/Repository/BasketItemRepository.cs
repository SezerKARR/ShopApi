namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;
public interface IBasketItemRepository: IRepository<BasketItem> {
    Task<List<BasketItem>> GetBasketItemsByBasketId(int basketId,int includes=-1);
}
[Flags]
public enum BasketItemInclude {
    Basket = 1,
    ProductSeller=2,
    
}

public class BasketItemRepository:BaseRepository<BasketItem>,IBasketItemRepository  {

    public BasketItemRepository(AppDbContext context) : base(context) {
    }
    protected override IQueryable<BasketItem> IncludeQuery(int includes = -1, IQueryable<BasketItem>? queryable = null) {
        var query =queryable ?? _queryable;
        if (includes != -1)
        {
            BasketItemInclude basketItemInclude=(BasketItemInclude)includes;
            if(basketItemInclude.HasFlag(BasketItemInclude.Basket))
                query.Include(bi=>bi.Basket).ThenInclude(b=>b.BasketItems);
            if(basketItemInclude.HasFlag(BasketItemInclude.ProductSeller))
                query.Include(bi=>bi.ProductSeller).ThenInclude(ps=>ps.Product);
                
        }
        return query;
    }
    public async Task<List<BasketItem>> GetBasketItemsByBasketId(int basketId,int includes = -1) {
        List<BasketItem> basketItems = await IncludeQuery(includes).Where(basketItem => basketItem.BasketId == basketId).ToListAsync();
        return basketItems;
    }
}

