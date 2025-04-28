namespace ShopApi.Interface;

public interface IRepository<T> where T : class, IEntity {

    IQueryable<T> GetQuery(int includes = -1);
    Task<T?> GetByIdAsync(int id, int includes = -1);
    Task<T?> GetBySlugAsync(string slug, int includes = -1);
    Task<List<T>?> GetAllAsync(int includes = -1);

    Task CreateAsync(T entity);
    Task<bool> AnyAsync(int id);

    void Update(T entity);

    void Delete(T entity);
}