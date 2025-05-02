namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public class BrandCategoryRepository:BaseRepository<BrandCategory>,IBrandCategoryRepository{

    public BrandCategoryRepository(AppDbContext context) : base(context) {
    }
    public async Task<List<Brand>> GetBrandsByCategoryIdAsync(int categoryId) {
        return await _context.BrandCategories
            .Where(bc => bc.CategoryId == categoryId)
            .Include(bc => bc.Brand)
            .Select(bc => bc.Brand)
            .Distinct()
            .ToListAsync();
    }
}

public interface IBrandCategoryRepository:IRepository<BrandCategory> {
    Task<List<Brand>> GetBrandsByCategoryIdAsync(int categoryId);
}