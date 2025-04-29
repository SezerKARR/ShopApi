namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

[Flags]
enum ProductSellerInclude {
    None = 0,

    Seller = 1,
    Product = 2,
    Stocks = 4,
    Comments = 8,

    All = Seller | Product | Stocks
}

public class ProductSellerRepository:BaseRepository<ProductSeller>, IProductSellerRepository {

    public ProductSellerRepository(AppDbContext context):base(context) {
    }
    protected override IQueryable<ProductSeller> IncludeQuery(int includes = -1, IQueryable<ProductSeller>? queryable = null) {
        var query = queryable ?? _queryable;
        if (includes != -1)
        {
            var productSellerInclude = (ProductSellerInclude)includes;
            if (productSellerInclude.HasFlag(ProductSellerInclude.Seller))
                query = query.Include(ps => ps.Seller).Where(ps => ps.Seller != null && ps.Seller.RoleInt == 2);
            if (productSellerInclude.HasFlag(ProductSellerInclude.Product))
                query = query.Include(ps => ps.Product);
            if (productSellerInclude.HasFlag(ProductSellerInclude.Stocks))
                query = query.Include(ps => ps.Stocks);
            if(productSellerInclude.HasFlag(ProductSellerInclude.Comments))
                query = query.Include(ps => ps.Comments);

        }
        return query;
    }
}

public interface IProductSellerRepository:IRepository<ProductSeller> {
}