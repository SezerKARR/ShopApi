namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

[Flags]
public enum ProductImageIncludes {
    Product = 1,
    Image = 2,
    
}
public interface IProductImageRepository:IRepository<ProductImage> {
    Task<List<ProductImage>?> GetProductImagesByProductIdAsync(int productId, int includes=-1);
}

public class ProductImageRepository:BaseRepository<ProductImage>, IProductImageRepository {

    public ProductImageRepository(AppDbContext context):base(context) {
    }
    protected override IQueryable<ProductImage> IncludeQuery(int includes = -1, IQueryable<ProductImage>? queryable = null) {


        var query = queryable ?? _dbSet.AsQueryable();
        if (includes != -1)
        {
            var productIncludes = (ProductImageIncludes)includes;

            if (productIncludes.HasFlag(ProductImageIncludes.Product))
                query = query.Include(pi => pi.Product);
            if(productIncludes.HasFlag(ProductImageIncludes.Image))
                query = query.Include(pi => pi.Image);
           
        }




        return query;

    }
    public async Task<List<ProductImage>?> GetProductImagesByProductIdAsync(int productId, int includes=-1) {
        return await IncludeQuery(includes).Where(pi => pi.ProductId == productId).ToListAsync();
    }
}
