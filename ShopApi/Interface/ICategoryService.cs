namespace ShopApi.Interface;

using Dtos.Category;
using Helpers;
using Models.Common;
using UpdateCategoryDto=Dtos.Category.UpdateCategoryDto;

public interface ICategoryService {
    /// <summary>
    /// Returns a queryable collection of categories
    /// </summary>
    Response<IQueryable<ReadCategoryDto>> GetQueryCategories();
    
    /// <summary>
    /// Gets all categories asynchronously
    /// </summary>
    Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync();
    
    /// <summary>
    /// Gets a category by its ID asynchronously
    /// </summary>
    Task<Response<ReadCategoryDto?>> GetCategoryByIdAsync(int id);
    
    /// <summary>
    /// Gets a category by its slug asynchronously
    /// </summary>
    Task<Response<ReadCategoryDto>> GetCategoryBySlugAsync(string slug);
    
    /// <summary>
    /// Gets categories with filtering, sorting, and pagination asynchronously
    /// </summary>
    Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync(QueryObject queryObject);
    
    /// <summary>
    /// Creates a new category asynchronously
    /// </summary>
    Task<Response<ReadCategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    
    /// <summary>
    /// Updates an existing category asynchronously
    /// </summary>
    Task<Response<ReadCategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    
    /// <summary>
    /// Deletes a category asynchronously
    /// </summary>
    Task<Response<ReadCategoryDto>> DeleteCategoryAsync(int id);
    
    /// <summary>
    /// Checks if a category exists asynchronously
    /// </summary>
    Task<Response<bool>> CategoryExistAsync(int id);
}