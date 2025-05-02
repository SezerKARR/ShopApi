namespace ShopApi.Repository;

using Abstracts;
using Microsoft.EntityFrameworkCore;
using Data;
using Interface;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly IQueryable<T> _queryable;
    
        protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _queryable = context.Set<T>().AsQueryable();
    }
 


    protected virtual IQueryable<T> IncludeQuery(int includes = -1,IQueryable<T>? queryable=null )
    {
        return _queryable; 
    }

    public IQueryable<T> GetQuery(int includes = -1)
    {
        return IncludeQuery(includes); 
    }

    public async Task<T?> GetByIdAsync(int id, int includes = -1)
    {
        var query = _queryable.AsQueryable();
        query = query.Where(x => x.Id == id); 
        query = IncludeQuery( includes,query);     
        return await query.FirstOrDefaultAsync();
       
    }

    public async Task<T?> GetBySlugAsync(string slug, int includes = -1)
    {
        var query = _queryable.AsQueryable();
        query = query.Where(x => x.Slug == slug); 
        query = IncludeQuery( includes,query);     
        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<T>?> GetAllAsync(int includes = -1)
    {
        return await IncludeQuery(includes:includes).ToListAsync();
    }

    
    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    public async Task<bool> AnyAsync(int id) {
        return await IncludeQuery().AnyAsync(x => x.Id == id);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
