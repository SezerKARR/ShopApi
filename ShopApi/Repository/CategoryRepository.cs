namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Common;

public class CategoryRepository(AppDbContext context) :ICategoryRepository{

    public IQueryable<Category> GetQueryCategories() {
        return context.Categories.AsQueryable();
    }
    public async Task<Category?> GetCategoryByIdAsync(int id) {
        List<Category> categories = await GetCategoriesAsync();
        Category category = categories.FirstOrDefault(x => x.Id == id);
        return category;
    }
    public async Task<Category?> GetCategoryBySlugAsync(string slug) {
        List<Category> categories =await GetCategoriesAsync();
        Category category = categories.FirstOrDefault(x => x.Slug == slug);
        return category;
    }
    public async Task<List<Category>> GetCategoriesAsync() {
        List<Category> categories = await context.Categories.ToListAsync();
        return categories;
    }
    public async Task CreateCategoryAsync(Category category) {
        await context.Categories.AddAsync(category);
    }
    public void UpdateCategory(Category category) {
        context.Entry(category).State = EntityState.Modified;
    }
    public void DeleteCategory(Category category) {
        context.Categories.Remove(category);
    }

}