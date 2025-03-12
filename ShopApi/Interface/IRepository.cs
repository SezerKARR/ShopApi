namespace ShopApi.Interface;

public interface IRepository<T> where T : class,IEntity {

        IQueryable<T>? GetQuery();

        Task<T?> GetTByIdAsync(int id);

        Task<T?> GetTBySlugAsync(string slug);

        Task<List<T>> GetAsync();

        Task CreateAsync(T entity);
        Task<bool> AnyAsync(int id);

        void Update(T entity);

        void Delete(T entity);
}