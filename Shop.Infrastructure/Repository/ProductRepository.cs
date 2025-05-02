namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Domain.Models.Request;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public interface IProductRepository:IRepository<Product> {

    Task<List<Product>?> GetProductsByCategoryIdAsync(int categoryId, int includes = -1);
    Task<List<Product>?> GetFilteredProducts(ProductFilterRequest productFilterRequest, int includes = -1);
}

[Flags]
public enum ProductIncludes {
    None = 0,

    Category = 1,
    Brand = 2,
    CreatedByUser = 4,
    ProductSeller = 16,
    ProductFilterValue = 32,
    ProductImages = 64,
    Comments = 128,
    All = Brand | Category | ProductFilterValue
}

public class ProductRepository:BaseRepository<Product>, IProductRepository {
    public ProductRepository(AppDbContext context):base(context) {
    }

    protected override IQueryable<Product> IncludeQuery(int includes = -1, IQueryable<Product>? queryable = null) {


        var query = queryable ?? _dbSet.AsQueryable();
        if (includes != -1)
        {
            var productIncludes = (ProductIncludes)includes;

            if (productIncludes.HasFlag(ProductIncludes.Category))
                query = query.Include(p => p.Category);
            if (productIncludes.HasFlag(ProductIncludes.Brand))
                query = query.Include(p => p.Brand);
            if (productIncludes.HasFlag(ProductIncludes.CreatedByUser))
                query = query.Include(p => p.CreatedBySeller);
            if (productIncludes.HasFlag(ProductIncludes.ProductSeller))
                query = query.Include(p => p.ProductSellers).ThenInclude(ps => ps.Seller).ThenInclude(s => s.Coupons);
            if (productIncludes.HasFlag(ProductIncludes.ProductFilterValue))
                query = query
                    .Include(p => p.FilterValues)
                    .ThenInclude(pv => pv.Filter)
                    .Include(p => p.FilterValues)
                    .ThenInclude(pv => pv.FilterValue);

            if (productIncludes.HasFlag(ProductIncludes.ProductImages))
                query = query.Include(p => p.ProductImages);
            if(productIncludes.HasFlag(ProductIncludes.Comments))
                query = query.Include(p => p.Comments);
        }




        return query;

    }
    // public async Task<List<Product>?> GetFilteredProducts(ProductFilterRequest filterRequest) {
    // 	try {
    // 		List<Product> filtered = await Queryable.ToListAsync();
    // 		if (filterRequest.FilterValueIds != null && filterRequest.FilterValueIds.Any())
    // 		{
    // 			var filterValueIdsList = filterRequest.FilterValueIds.ToList();
    //
    // 			filtered =  filtered.Where(p => p.FilterValues
    // 				.Any(fv => filterValueIdsList.Contains(fv.FilterValueId))).ToList();
    //
    // 		}
    // 		if (filterRequest.CategoryId.HasValue) { filtered = filtered.Where(p => p.CategoryId == filterRequest.CategoryId.Value).ToList(); }
    //
    // 		if (filterRequest.BrandId.HasValue) { filtered = filtered.Where(p => p.BrandId == filterRequest.BrandId.Value).ToList(); }
    //
    // 		if (!string.IsNullOrEmpty(filterRequest.SearchQuery)) { filtered = filtered.Where(p => p.Name != null && p.Name.Contains(filterRequest.SearchQuery)).ToList(); }
    //
    // 		if (!string.IsNullOrEmpty(filterRequest.SortBy))
    // 		{
    // 			filtered = filterRequest.Ascending
    // 				? filtered.OrderBy(p => EF.Property<object>(p, filterRequest.SortBy)).ToList()
    // 				: filtered.OrderByDescending(p => EF.Property<object>(p, filterRequest.SortBy)).ToList();
    // 		}
    // 		return filtered;
    // 		
    // 	}
    // 	catch (Exception e)
    // 	{
    // 		Console.WriteLine(e);
    // 		throw;
    // 	}
    // 	
    // }
    public async Task<List<Product>?> GetFilteredProducts(ProductFilterRequest productFilterRequest, int includes = -1) {
        try
        {
            var query = IncludeQuery(includes);
            if (productFilterRequest.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productFilterRequest.CategoryId.Value);
            }
            if (productFilterRequest.GroupedFilterValues != null)
                foreach (var group in productFilterRequest.GroupedFilterValues)
                {
                    var valueIds = group.Value;


                    query = query.AsEnumerable().Where(p => p.FilterValues
                        .Any(fv => valueIds.Contains(fv.FilterValueId))).AsQueryable();

                }



            if (productFilterRequest.MinPrice != -1)
            {
                query = query.Where(p => p.MinPrice >= productFilterRequest.MinPrice);
            }

            if (productFilterRequest.MaxPrice != -1)
            {
                query = query.Where(p => p.MinPrice <= productFilterRequest.MaxPrice);
            }
            if (productFilterRequest.BrandIds?.Length > 0)
            {
                query = query.AsEnumerable().Where(p => productFilterRequest.BrandIds.Contains(p.BrandId)).AsQueryable();

            }

            if (!string.IsNullOrEmpty(productFilterRequest.SearchQuery))
            {
                query = query.Where(p => p.Name != null && p.Name.Contains(productFilterRequest.SearchQuery));
            }

            if (!string.IsNullOrEmpty(productFilterRequest.SortBy))
            {
                query = productFilterRequest.Ascending
                    ? query.OrderBy(p => EF.Property<object>(p, productFilterRequest.SortBy))
                    : query.OrderByDescending(p => EF.Property<object>(p, productFilterRequest.SortBy));
            }

            return query.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<List<Product>?> GetProductsByCategoryIdAsync(int categoryId, int includes = -1) {
        return await IncludeQuery(includes).Where(p => p.CategoryId == categoryId).ToListAsync();
    }
}