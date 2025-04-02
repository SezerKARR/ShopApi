namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository {

    public CategoryRepository(AppDbContext context) : base(context) {
        _queryable = _context.Categories.AsQueryable();
    }
    protected override IQueryable<Category> Include() {
        Console.WriteLine("CategoryRepository");
        return _dbSet.Include(c => c.Products).Include(c => c.SubCategories).
            Include(c => c.Filters).AsQueryable();
    }


}