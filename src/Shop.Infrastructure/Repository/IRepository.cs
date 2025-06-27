namespace Shop.Infrastructure.Repository;

using Domain.Models;

public interface IRepository<T> where T : class, IEntity {

    public IQueryable<T> GetQuery(int includes = -1);
    public Task<T?> GetByIdAsync(int id, int includes = -1);
    public Task<T?> GetBySlugAsync(string slug, int includes = -1);
    public Task<List<T>?> GetAllAsync(int includes = -1);

    public Task CreateAsync(T entity);
    public Task<bool> AnyAsync(int id);

    public void Update(T entity);

    public void Delete(T entity);
}