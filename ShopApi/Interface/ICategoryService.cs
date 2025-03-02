namespace ShopApi.Interface;

using Dtos.Category;
using Models;
using Models.Common;
using SaplingStore.Helpers;

public interface ICategoryService {
    /// <summary>
    /// Returns a queryable collection of categories
    /// </summary>
    Response<IQueryable<Category>> GetQueryCategories();
    
    /// <summary>
    /// Gets all categories asynchronously
    /// </summary>
    Task<Response<List<Category>>> GetCategoriesAsync();
    
    /// <summary>
    /// Gets a category by its ID asynchronously
    /// </summary>
    Task<Response<Category>> GetCategoryByIdAsync(int id);
    
    /// <summary>
    /// Gets a category by its slug asynchronously
    /// </summary>
    Task<Response<Category>> GetCategoryBySlugAsync(string slug);
    
    /// <summary>
    /// Gets categories with filtering, sorting, and pagination asynchronously
    /// </summary>
    Task<Response<List<Category>>> GetCategoriesAsync(QueryObject queryObject);
    
    /// <summary>
    /// Creates a new category asynchronously
    /// </summary>
    Task<Response<Category>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    
    /// <summary>
    /// Updates an existing category asynchronously
    /// </summary>
    Task<Response<Category>> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    
    /// <summary>
    /// Deletes a category asynchronously
    /// </summary>
    Task<Response<Category>> DeleteCategoryAsync(int id);
    
    /// <summary>
    /// Checks if a category exists asynchronously
    /// </summary>
    Task<Response<bool>> CategoryExistAsync(int id);
}