namespace ShopApi.Repository;

using Interface;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Request;

public interface IProductRepository : IRepository<Product> {
	Task<List<Product>?> GetProductsByFilterValueIdsAsync(List<int> filterValueIds);

	Task<List<Product>?> GetProductsByCategoryIdAsync(int categoryId);
	Task<List<Product>?> GetFilteredProducts(FilterRequest filterRequest);
}

public class ProductRepository : BaseRepository<Product>, IProductRepository {
	public ProductRepository(AppDbContext context) : base(context) {
	}

	protected override IQueryable<Product> Include() {
		Console.WriteLine("CategoryRepository");
		return _dbSet.Include(p => p.FilterValues).AsQueryable();
	}
	// public async Task<List<Product>?> GetFilteredProducts(FilterRequest filterRequest) {
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
	public async Task<List<Product>?> GetFilteredProducts(FilterRequest filterRequest)
	{
		try
		{
			var query = Queryable;
			if (filterRequest.GroupedFilterValues != null)
				foreach (var group in filterRequest.GroupedFilterValues)
				{
					var valueIds = group.Value;
					

					query = query.AsEnumerable().Where(p => p.FilterValues
						.Any(fv => valueIds.Contains(fv.FilterValueId))).AsQueryable();

				}
		

			if (filterRequest.CategoryId.HasValue)
			{
				query = query.Where(p => p.CategoryId == filterRequest.CategoryId.Value);
			}

			if (filterRequest.BrandId.HasValue)
			{
				query = query.Where(p => p.BrandId == filterRequest.BrandId.Value);
			}

			if (!string.IsNullOrEmpty(filterRequest.SearchQuery))
			{
				query = query.Where(p => p.Name != null && p.Name.Contains(filterRequest.SearchQuery));
			}

			if (!string.IsNullOrEmpty(filterRequest.SortBy))
			{
				query = filterRequest.Ascending
					? query.OrderBy(p => EF.Property<object>(p, filterRequest.SortBy))
					: query.OrderByDescending(p => EF.Property<object>(p, filterRequest.SortBy));
			}

			return  query.ToList();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public async Task<List<Product>?> GetProductsByFilterValueIdsAsync(List<int> filterValueIds) {
		return await Queryable.Where(p => p.FilterValues.Any(pfv => pfv.FilterValueId != null && filterValueIds.Contains((int)pfv.FilterValueId))).ToListAsync();
	}
	public async Task<List<Product>?> GetProductsByCategoryIdAsync(int categoryId) {
		return await Queryable.Where(p => p.CategoryId == categoryId).ToListAsync();
	}
}