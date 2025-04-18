namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class FilterRepository:BaseRepository<Filter>,IFilterRepository{

    public FilterRepository(AppDbContext context) : base(context) {
    }
    public async Task<List<Filter>> GetFiltersByCategoryId(int categoryId) {
        return await Queryable.Where(filter => filter.CategoryId == categoryId).ToListAsync();
    }
    protected override IQueryable<Filter> Include() {
        return _dbSet.Include(f => f.Values).AsQueryable();
    }
}

public interface IFilterRepository:IRepository<Filter> {
    public Task<List<Filter>> GetFiltersByCategoryId(int categoryId);
}