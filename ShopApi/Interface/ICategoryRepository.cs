namespace ShopApi.Interface;

using Models;
using Models.Common;

public interface ICategoryRepository {
    IQueryable<Category> GetQueryCategories();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category?> GetCategoryBySlugAsync(string slug);
    Task<List<Category>> GetCategoriesAsync();
    Task CreateCategoryAsync(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(Category category);

}