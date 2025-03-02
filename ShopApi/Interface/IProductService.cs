namespace ShopApi.Interface
{
using Dtos.Product;
using Models;
using Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaplingStore.Helpers;

public interface IProductService
{
    Task<Response<List<Product>>> GetProductsAsync();
    Response<IQueryable<Product>> GetProductsQueryable();
    Task<Response<Product>> GetProductByIdAsync(int id);
    Task<Response<Product>> GetProductBySlugAsync(string slug);
    Task<Response<List<Product>>> GetProductsAsync(QueryObject queryObject);
    Task<Response<Product>> CreateProductAsync(CreateProductDto createProductDto);
    Task<Response<Product>> UpdateProductAsync(int id, UpdateProductDto dto);
    Task<Response<Product>> DeleteProductAsync(int id);
    Task<Response<bool>> ProductExistAsync(int id);
}
}