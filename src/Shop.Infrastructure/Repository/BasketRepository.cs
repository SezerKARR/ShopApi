namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public interface IBasketRepository:IRepository<Basket> {
    Task<Basket?> GetBasketByUserIdAsync(int? userId, int includes = -1);
}

[Flags]
public enum BasketIncludes {
    BasketItems = 1,
    User = 2,
}

public class BasketRepository:BaseRepository<Basket>, IBasketRepository {

    public BasketRepository(AppDbContext context):base(context) {
    }
    protected override IQueryable<Basket> IncludeQuery(int includes = -1, IQueryable<Basket>? queryable = null) {
        var query = queryable ?? _dbSet.AsQueryable();
        if (includes != -1)
        {

            BasketIncludes basketIncludes = (BasketIncludes)includes;
            if (basketIncludes.HasFlag(BasketIncludes.BasketItems))
                query = query.Include(basket => basket.BasketItems) // Ana navigasyon
                    .ThenInclude(bi => bi.ProductSeller) // ProductSeller'a git
                    .ThenInclude(ps => ps.Product) // ProductSeller'dan Product'a git
                    .ThenInclude(p => p.Brand) // Product'tan Brand'e git
                    .Include(basket => basket.BasketItems) // Ana navigasyona DÖN (Product'ın diğer özelliği için)
                    .ThenInclude(bi => bi.ProductSeller)
                    .ThenInclude(ps => ps.Product)
                    .ThenInclude(p => p.ProductImages.OrderBy(pi=>pi.Order)) // Product'tan ProductImages'a git
                    .ThenInclude(pi => pi.Image) // ProductImage'dan Image'a git
                    .Include(basket => basket.BasketItems) // Ana navigasyona DÖN (ProductSeller'ın diğer özelliği için)
                    .ThenInclude(bi => bi.ProductSeller)
                    .ThenInclude(ps => ps.Seller);
            if (basketIncludes.HasFlag(BasketIncludes.User))
                query = query.Include(basket => basket.User);

        }
        return query;
    }

    public async Task<Basket?> GetBasketByUserIdAsync(int? userId, int includes = -1) {
        Basket? basket = await IncludeQuery(includes).FirstOrDefaultAsync(basket => basket.UserId == userId);
        return basket;
    }
}