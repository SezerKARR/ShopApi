namespace ShopApi.Interface;

using Dtos.Product;
using Models;
using SaplingStore.Helpers;

public interface IProductService {
    public Task<List<ReadProductDto>> GetProductsAsync();
    public Task<ReadProductDto?> GetProductByIdAsync(int id);
    public Task<ReadProductDto?> GetProductBySlugAsync(string slug);
    public Task<List<ReadProductDto>> GetProductsAsync(QueryObject queryObject);
    public Task<ReadProductDto> CreateProductAsync(CreateProductDto product);
    public Task<ReadProductDto?> UpdateProductAsync(int id, UpdateProductDto dto);
    public Task<ReadProductDto?> DeleteProductAsync(int id);
    public Task<bool> ProductExistAsync(int id);
}