namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Category;
using Helpers;
using Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Shared.Cache;
using UpdateCategoryDto=Dtos.Category.UpdateCategoryDto;

public class CategoryService(IMapper mapper, AppDbContext context,ICategoryRepository categoryRepository,IMemoryCache memoryCache,IUnitOfWork unitOfWork,ILogger<CategoryService> logger) : ICategoryService {
   
    public  Response<IQueryable<ReadCategoryDto>> GetQueryCategories() {
        try
        {
            var queryCategories = categoryRepository.GetQuery();
            IQueryable<ReadCategoryDto> readCategoryDtosQuery= mapper.Map<IQueryable<ReadCategoryDto>>(queryCategories);
            return new Response<IQueryable<ReadCategoryDto>>(readCategoryDtosQuery);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching product query.");
            return new Response<IQueryable<ReadCategoryDto>>($"An error occurred while fetching product query: {ex.Message}");
        }
    }
    public async Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync() {
        try
        {
            List<Category>? categories = await memoryCache.GetOrCreateAsync(CacheKeys.CategoriesList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return categoryRepository.GetAsync();
            });
            List<ReadCategoryDto> readCategoryDtos = mapper.Map<List<ReadCategoryDto>>(categories);
            return new Response<List<ReadCategoryDto>>(readCategoryDtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Categoryies.");
            return new Response<List<ReadCategoryDto>>($"An error occurred while fetching Categories: {ex.Message}");
        }
    }
    public async Task<Response<ReadCategoryDto?>> GetCategoryByIdAsync(int id) {
        try
        {
            // Ürün listesini Response ile alıyoruz
            Category? category= await categoryRepository.GetTByIdAsync(id);

            if (category == null) { return new Response<ReadCategoryDto?>($"Category with id: {id} not found."); }
            ReadCategoryDto readCategoryDto = mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto?>(readCategoryDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Category by id.");
            return new Response<ReadCategoryDto?>($"An error occurred: {ex.Message}");
        }
    }
 
    public async Task<Response<ReadCategoryDto>> GetCategoryBySlugAsync(string slug) { 
        try
        {
            var category = await categoryRepository.GetTBySlugAsync(slug);

            if (category == null) { return new Response<ReadCategoryDto>($"Category with slug: {slug} not found."); }
            ReadCategoryDto readCategoryDto = mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching Category by slug.");
            return new Response<ReadCategoryDto>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync(QueryObject queryObject) {
        try
        {
            IQueryable<ReadCategoryDto>? query = GetQueryCategories().Resource;
            if (query == null)
            {
                return new Response<List<ReadCategoryDto>>($"An error occurred while fetching categories");// Hata mesajı döndür
            }
            // Filtreleme ve sıralama işlemleri
            query = QueryableExtensions.ApplyFilter(query, queryObject.SortBy, queryObject.FilterBy);
            query = QueryableExtensions.ApplySorting(query, queryObject.SortBy, queryObject.IsDecSending);

            // Sayfalama
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
            var categories = await query.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

            return new Response<List<ReadCategoryDto>>(categories);// Başarılı şekilde döndür
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching categories.");
            return new Response<List<ReadCategoryDto>>($"An error occurred while fetching categories: {ex.Message}");// Hata mesajı döndür
        }
    }
    public async Task<Response<ReadCategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto) {
        try
        {
            Category category = mapper.Map<Category>(createCategoryDto);
            category.Slug = SlugHelper.GenerateSlug(category.Name);
          
            await categoryRepository.CreateAsync(category);
            await unitOfWork.CommitAsync();
            memoryCache.Remove(CacheKeys.CategoriesList);
            ReadCategoryDto readCategoryDto = mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not save category.");
            return new Response<ReadCategoryDto>($"An error occurred when saving the category: {ex.Message}");
        }
    }
    public async Task<Response<ReadCategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto) 
    {
        var existingCategory = await categoryRepository.GetTByIdAsync(id);
        if (existingCategory == null) 
        {
            return new Response<ReadCategoryDto>($"Category with id: {id} not found.");
        }

        try
        {
            mapper.Map(dto, existingCategory);

            if (!string.IsNullOrEmpty(dto.Name)) 
            {
                existingCategory.Slug = SlugHelper.GenerateSlug(existingCategory.Name);
            }

            categoryRepository.Update(existingCategory);
        
            await unitOfWork.CommitAsync();

            memoryCache.Remove(CacheKeys.CategoriesList);

            var updatedCategoryDto = mapper.Map<ReadCategoryDto>(existingCategory);
            return new Response<ReadCategoryDto>(updatedCategoryDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not update Category with ID {id}.", id);
            return new Response<ReadCategoryDto>($"An error occurred when updating the Category: {ex.Message}");
        }
    }
    public async Task<Response<ReadCategoryDto>> DeleteCategoryAsync(int id) {
        var existCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (existCategory == null) return new Response<ReadCategoryDto>("Product not found.");
        try
        {
            categoryRepository.Delete(existCategory);
            await unitOfWork.CommitAsync();
            memoryCache.Remove(CacheKeys.CategoriesList);
            ReadCategoryDto readCategoryDto = mapper.Map<ReadCategoryDto>(existCategory);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not delete Category with ID {id}.", id);
            return new Response<ReadCategoryDto>($"An error occurred when deleting the Category: {e.Message}");
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
   
  
}

