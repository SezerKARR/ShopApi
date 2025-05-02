namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

[Flags]
enum BrandIncludes {
    None=0,
    BrandCategories=1,
    Products=2,
    BrandAdmins=4,
    All=BrandCategories | Products | BrandAdmins
    
}
public class BrandRepository:BaseRepository<Brand>,IBrandRepository {

    public BrandRepository(AppDbContext context) : base(context) {
    }
    protected override IQueryable<Brand> IncludeQuery(int includes=-1,IQueryable<Brand>? queryable=null) {
        var query =queryable ?? _dbSet.AsQueryable();
        if (includes != -1)
        {
            var brandIncludes = (BrandIncludes)includes;
            if(brandIncludes.HasFlag(BrandIncludes.BrandCategories))
                query = query.Include(b => b.BrandCategories);
            if(brandIncludes.HasFlag(BrandIncludes.Products))
                query = query.Include(b => b.Products);
            if(brandIncludes.HasFlag(BrandIncludes.BrandAdmins))
                query = query.Include(b => b.BrandAdmins);
            
        }
        return query;
    }

  
}

public interface IBrandRepository:IRepository<Brand> {

    
}