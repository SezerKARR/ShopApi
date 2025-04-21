namespace ShopApi.Repository;

using Interface;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Request;

public interface IProductRepository:IRepository<Product> {

    Task<List<Product>?> GetProductsByCategoryIdAsync(int categoryId);
    Task<List<Product>?> GetFilteredProducts(ProductFilterRequest productFilterRequest);
}

public class ProductRepository:BaseRepository<Product>, IProductRepository {
    public ProductRepository(AppDbContext context):base(context) {
    }

    protected override IQueryable<Product> Include() {
        Console.WriteLine("CategoryRepository");
        return _dbSet.Include(p => p.FilterValues).AsQueryable();
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
    public async Task<List<Product>?> GetFilteredProducts(ProductFilterRequest productFilterRequest) {
        try
        {
            var query = Queryable;
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


           
            if (productFilterRequest.MinPrice!=-1)
            {
                query = query.Where(p => p.Price >= productFilterRequest.MinPrice);
            }

            if (productFilterRequest.MaxPrice!=-1)
            {
                query = query.Where(p => p.Price <= productFilterRequest.MaxPrice);
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


    public async Task<List<Product>?> GetProductsByCategoryIdAsync(int categoryId) {
        return await Queryable.Where(p => p.CategoryId == categoryId).ToListAsync();
    }
}