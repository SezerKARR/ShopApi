namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

[Flags]
enum CategoryIncludes {
    None = 0,
    ParentCategory=1,
    SubCategories=2,
    Filters=4,
    Products=8,
    BrandCategories=16,
    All=ParentCategory | SubCategories | Filters | Products | BrandCategories
}
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository {

    public CategoryRepository(AppDbContext context) : base(context) {
    }
    protected override IQueryable<Category> IncludeQuery(int includes=-1,IQueryable<Category>? queryable=null) {
        var query =queryable ?? _dbSet.AsQueryable();
        if (includes != -1)
        {
            var categoryIncludes = (CategoryIncludes)includes;
            
            if (categoryIncludes.HasFlag(CategoryIncludes.ParentCategory))
                query = query.Include(x => x.ParentCategory);
            if(categoryIncludes.HasFlag(CategoryIncludes.SubCategories))
                query = query.Include(x => x.SubCategories);
            if(categoryIncludes.HasFlag(CategoryIncludes.Filters))
                query = query.Include(x => x.Filters);
            if(categoryIncludes.HasFlag(CategoryIncludes.Products))
                query = query.Include(x => x.Products);
            if(categoryIncludes.HasFlag(CategoryIncludes.BrandCategories))
                query = query.Include(x => x.BrandCategories);
            
        }
        return query;
    }


}