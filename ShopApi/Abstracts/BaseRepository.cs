namespace ShopApi.Abstracts;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    internal readonly DbSet<T> dbSet;
    IQueryable<T> Queryable;
    public BaseRepository(AppDbContext context)
    {
        _context = context;
        dbSet = context.Set<T>();
        Queryable=Include(context.Set<T>().AsQueryable());
    }

    protected virtual IQueryable<T> Include(IQueryable<T> query) => query;

    public IQueryable<T> GetQuery() => Queryable;

    public async Task<T?> GetTByIdAsync(int id)
    {
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
        await dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        dbSet.Remove(entity);
    }
}
