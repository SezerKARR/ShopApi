namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Category;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using SaplingStore.Helpers;

public class CategoryService(IMapper mapper, AppDbContext context) : ICategoryService {
   
    public Task<List<ReadCategoryDto>> GetCategoriesAsync() => throw new NotImplementedException();
    public Task<ReadCategoryDto?> GetCategoryByIdAsync(int id) => throw new NotImplementedException();
    public Task<ReadCategoryDto?> GetCategoryBySlugAsync(string slug) => throw new NotImplementedException();
    public Task<List<ReadCategoryDto>> GetCategoriesAsync(QueryObject queryObject) => throw new NotImplementedException();
    public async Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto) {
        Category category= mapper.Map<Category>(createCategoryDto);
        category.Slug = SlugHelper.GenerateSlug(category.Name);
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return mapper.Map<ReadCategoryDto>(category);
    }
    public Task<ReadCategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto) => throw new NotImplementedException();
    public Task<ReadCategoryDto?> DeleteCategoryAsync(int id) => throw new NotImplementedException();
    public Task<bool> CategoryExistAsync(int id) {
        Console.WriteLine("Create Product");
        return context.Categories.AnyAsync(category => category.Id == id);
    }
}

