namespace ShopApi.Interface;

using Dtos.Category;
using Models;
using SaplingStore.Helpers;

public interface ICategoryService {
    public Task<List<ReadCategoryDto>> GetCategoriesAsync();
    public Task<ReadCategoryDto?> GetCategoryByIdAsync(int id);
    public Task<ReadCategoryDto?> GetCategoryBySlugAsync(string slug);
    public Task<List<ReadCategoryDto>> GetCategoriesAsync(QueryObject queryObject);
    public Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto category);
    public Task<ReadCategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    public Task<ReadCategoryDto?> DeleteCategoryAsync(int id);
    public Task<bool> CategoryExistAsync(int id);
}