namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

[Flags]
public enum FilterIncludes
{
    None = 0,
    Category = 1,
    FilterValue = 2,
    All =  Category | FilterValue
}
public class FilterRepository:BaseRepository<Filter>,IFilterRepository{

    public FilterRepository(AppDbContext context) : base(context) {
    }
    public async Task<List<Filter>> GetFiltersByCategoryId(int categoryId,int includes=-1) {
        return await IncludeQuery(includes).Where(filter => filter.CategoryId == categoryId).ToListAsync();
    }
    protected override IQueryable<Filter> IncludeQuery(int includes=-1,IQueryable<Filter>? queryable=null) {
        var query =queryable ?? _dbSet.AsQueryable();
        if (includes != -1)
        {
            var filterIncludes = (FilterIncludes)includes;
            if(filterIncludes.HasFlag(FilterIncludes.Category))
                query=query.Include(f=>f.Category);
            if(filterIncludes.HasFlag(FilterIncludes.FilterValue))
                query=query.Include(f=>f.Values);
        }
        return query;
    }
}

public interface IFilterRepository:IRepository<Filter> {
    public Task<List<Filter>> GetFiltersByCategoryId(int categoryId,int includes=-1);
}