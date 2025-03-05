namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Category;
using Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using SaplingStore.Helpers;
using Shared.Cache;

public class CategoryService(IMapper mapper, AppDbContext context,ICategoryRepository categoryRepository,IMemoryCache memoryCache,IUnitOfWork unitOfWork,ILogger<CategoryService> logger) : ICategoryService {
   
    public  Response<IQueryable<Category>> GetQueryCategories() {
        try
        {
            var queryCategories = categoryRepository.GetQuery();
            return new Response<IQueryable<Category>>(queryCategories);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching product query.");
            return new Response<IQueryable<Category>>($"An error occurred while fetching product query: {ex.Message}");
        }
    }
    public async Task<Response<List<Category>>> GetCategoriesAsync() {
        try
        {
            var categories = await memoryCache.GetOrCreateAsync(CacheKeys.CategoriesList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return categoryRepository.GetAsync();
            });
            Console.WriteLine(categories[1].Products.Count);
            return new Response<List<Category>>(categories ?? new List<Category>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Categoryies.");
            return new Response<List<Category>>($"An error occurred while fetching Categories: {ex.Message}");
        }
    }
    public async Task<Response<Category>> GetCategoryByIdAsync(int id) {
        try
        {
            // Ürün listesini Response ile alıyoruz
            var category= await categoryRepository.GetTByIdAsync(id);

            if (category == null) { return new Response<Category>($"Category with id: {id} not found."); }

            return new Response<Category>(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Category by id.");
            return new Response<Category>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<Category>> GetCategoryBySlugAsync(string slug) { 
        try
        {
            var category = await categoryRepository.GetTBySlugAsync(slug);

            if (category == null) { return new Response<Category>($"Category with slug: {slug} not found."); }

            return new Response<Category>(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Category by slug.");
            return new Response<Category>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<List<Category>>> GetCategoriesAsync(QueryObject queryObject) {
        try
        {
            var query = GetQueryCategories().Resource;

            // Filtreleme ve sıralama işlemleri
            query = QueryableExtensions.ApplyFilter(query, queryObject.SortBy, queryObject.FilterBy);
            query = QueryableExtensions.ApplySorting(query, queryObject.SortBy, queryObject.IsDecSending);

            // Sayfalama
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
            var categories = await query.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

            return new Response<List<Category>>(categories);// Başarılı şekilde döndür
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching categories.");
            return new Response<List<Category>>($"An error occurred while fetching categories: {ex.Message}");// Hata mesajı döndür
        }
    }
    public async Task<Response<Category>> CreateCategoryAsync(CreateCategoryDto createCategoryDto) {
        try
        {
            Category category = mapper.Map<Category>(createCategoryDto);
            category.Slug = SlugHelper.GenerateSlug(category.Name);
          
            await categoryRepository.CreateAsync(category);
            await unitOfWork.CommitAsync();
            memoryCache.Remove(CacheKeys.CategoriesList);
            return new Response<Category>(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not save category.");
            return new Response<Category>($"An error occurred when saving the category: {ex.Message}");
        }
    }
    public async Task<Response<Category>> UpdateCategoryAsync(int id, UpdateCategoryDto dto) {
        var existingCategory = await GetCategoryByIdAsync(id);
        Category category = existingCategory.Resource;
        if(category==null) { return new Response<Category>($"Category with id: {id} not found."); }
        try
        {
            mapper.Map(dto, category);
            categoryRepository.Update(category);
            if (dto.Name != "")
            {
                category.Slug = SlugHelper.GenerateSlug(category.Name);
            }
            await unitOfWork.CommitAsync();
            memoryCache.Remove(CacheKeys.CategoriesList);
            return new Response<Category>(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not update Category with ID {id}.", id);
            return new Response<Category>($"An error occurred when updating the Category: {ex.Message}");
        }
    }
    public async Task<Response<Category>> DeleteCategoryAsync(int id) {
        var existCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (existCategory == null) return new Response<Category>("Product not found.");
        try
        {
            categoryRepository.Delete(existCategory);
            await unitOfWork.CommitAsync();
            memoryCache.Remove(CacheKeys.CategoriesList);
            return new Response<Category>(existCategory);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not delete Category with ID {id}.", id);
            return new Response<Category>($"An error occurred when deleting the Category: {e.Message}");
        }
    }
    public async Task<Response<bool>> CategoryExistAsync(int id) {
        try
        {
            // Ürünün var olup olmadığını kontrol et
            var exists = await context.Categories.AnyAsync(e => e.Id == id);

            if (exists)
            {
                return new Response<bool>(true); // Ürün bulunduysa başarılı cevap döndür
            }
            else
            {
                return new Response<bool>(false); // Ürün bulunmadıysa başarısız cevap döndür
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while checking if Category exists.");
            return new Response<bool>($"An error occurred while checking if Category exists: {ex.Message}"); // Hata mesajı döndür
        }
    }
   
    
    // public async Task<ReadCategoryDto?> GetCategoryByIdAsync(int id) {
    //     Category? category = await context.Categories.FirstOrDefaultAsync(category=> category.Id == id);
    //     if (category == null)
    //     {
    //         return null;
    //     }
    //     ReadCategoryDto? dto = mapper.Map<ReadCategoryDto>(category);
    //     return dto;
    // }
    // public Task<ReadCategoryDto?> GetCategoryBySlugAsync(string slug) => throw new NotImplementedException();
    // public Task<List<ReadCategoryDto>> GetAsync(QueryObject queryObject) => throw new NotImplementedException();
    // public async Task<ReadCategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto) {
    //     Category? category= mapper.Map<Category>(createCategoryDto);
    //     category.Slug = SlugHelper.GenerateSlug(category.Name);
    //     await context.Categories.AddAsync(category);
    //     await context.SaveChangesAsync();
    //     return mapper.Map<ReadCategoryDto>(category);
    // }
    // public async Task<ReadCategoryDto?> UpdateCategoryAsync(int id,UpdateCategoryDto dto) {
    //     Category? category = await context.Categories.FirstOrDefaultAsync(category=> category.Id == id);
    //     
    //     
    // }
    // public Task<ReadCategoryDto?> DeleteCategoryAsync(int id) => throw new NotImplementedException();
    // public Task<bool> CategoryExistAsync(int id) {
    //     Console.WriteLine("Create Product");
    //     return context.Categories.AnyAsync(category => category.Id == id);
    // }
  
}

