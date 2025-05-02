namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public interface IProductImageRepository:IRepository<ProductImage> {
    Task<List<ProductImage>?> GetProductImagesByProductIdAsync(int productId, int includes=-1);
}

public class ProductImageRepository:BaseRepository<ProductImage>, IProductImageRepository {

    public ProductImageRepository(AppDbContext context):base(context) {
    }
    public async Task<List<ProductImage>?> GetProductImagesByProductIdAsync(int productId, int includes=-1) {
        return await IncludeQuery(includes).Where(pi => pi.ProductId == productId).ToListAsync();
    }
}
