namespace ShopApi.Interface;

using Models;

public interface IProductRepository {
    IQueryable<Product> GetProductsQueryable();
    Task<List<Product>> GetProductsAsync();

    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductBySlugAsync(string slug);

    Task CreateProductAsync(Product product);

    void UpdateProduct(Product product);

    void DeleteProduct(Product product);
}