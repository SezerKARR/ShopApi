namespace ShopApi.Repository;

using Abstracts;
using Microsoft.EntityFrameworkCore;
using Data;
using Interface;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet; 
    protected IQueryable<T>? _queryable;

    protected IQueryable<T> Queryable => Include();
    
        protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
   

    protected virtual IQueryable<T> Include() => _dbSet.AsQueryable();

    public IQueryable<T>? GetQuery() => _queryable;

    public async Task<T?> GetTByIdAsync(int id)
    {
        Console.WriteLine(_queryable);
        return await Queryable.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T?> GetTBySlugAsync(string slug)
    {
        return await Queryable.FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<List<T>> GetAsync()
    {
        return await Queryable.ToListAsync();
    }
    
    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    public async Task<bool> AnyAsync(int id) {
        return await Queryable.AnyAsync(x => x.Id == id);
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
