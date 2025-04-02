namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class FilterRepository:BaseRepository<Filter>,IFilterRepository{

    public FilterRepository(AppDbContext context) : base(context) {
    }
    protected override IQueryable<Filter> Include() {
        return _dbSet.Include(f => f.Values).AsQueryable();
    }
}

public interface IFilterRepository:IRepository<Filter> {
}