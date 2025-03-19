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
public interface ICategoryService {
    Response<IQueryable<ReadCategoryDto>> GetQueryCategories();
    Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync();
    Task<Response<ReadCategoryDto?>> GetCategoryByIdAsync(int id);
    Task<Response<ReadCategoryDto>> GetCategoryBySlugAsync(string slug);
    Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync(QueryObject queryObject);
    Task<Response<ReadCategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<Response<ReadCategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    Task<Response<ReadCategoryDto>> DeleteCategoryAsync(int id);
    Task<Response<bool>> CategoryExistAsync(int id);
}
public class CategoryService : ICategoryService {
    readonly IMapper _mapper;
    readonly AppDbContext _context;
    readonly ICategoryRepository _categoryRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<CategoryService> _logger;
    public CategoryService(IMapper mapper, AppDbContext context,ICategoryRepository categoryRepository,IMemoryCache memoryCache,IUnitOfWork unitOfWork,ILogger<CategoryService> logger) {
        _mapper = mapper;
        _context = context;
        _categoryRepository = categoryRepository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public  Response<IQueryable<ReadCategoryDto>> GetQueryCategories() {
        try
        {
            var queryCategories = _categoryRepository.GetQuery();
            IQueryable<ReadCategoryDto> readCategoryDtosQuery= _mapper.Map<IQueryable<ReadCategoryDto>>(queryCategories);
            return new Response<IQueryable<ReadCategoryDto>>(readCategoryDtosQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product query.");
            return new Response<IQueryable<ReadCategoryDto>>($"An error occurred while fetching product query: {ex.Message}");
        }
    }
    public async Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync() {
        try
        {
            List<Category>? categories = await _memoryCache.GetOrCreateAsync(CacheKeys.CategoriesList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _categoryRepository.GetAsync();
            });
            List<ReadCategoryDto> readCategoryDtos = _mapper.Map<List<ReadCategoryDto>>(categories);
            return new Response<List<ReadCategoryDto>>(readCategoryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Categoryies.");
            return new Response<List<ReadCategoryDto>>($"An error occurred while fetching Categories: {ex.Message}");
        }
    }
    public async Task<Response<ReadCategoryDto?>> GetCategoryByIdAsync(int id) {
        try
        {
            Category? category= await _categoryRepository.GetTByIdAsync(id);

            if (category == null) { return new Response<ReadCategoryDto?>($"Category with id: {id} not found."); }
            ReadCategoryDto readCategoryDto = _mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto?>(readCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Category by id.");
            return new Response<ReadCategoryDto?>($"An error occurred: {ex.Message}");
        }
    }
 
    public async Task<Response<ReadCategoryDto>> GetCategoryBySlugAsync(string slug) { 
        try
        {
            var category = await _categoryRepository.GetTBySlugAsync(slug);

            if (category == null) { return new Response<ReadCategoryDto>($"Category with slug: {slug} not found."); }
            ReadCategoryDto readCategoryDto = _mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Category by slug.");
            return new Response<ReadCategoryDto>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync(QueryObject queryObject) {
        try
        {
            IQueryable<ReadCategoryDto>? query = GetQueryCategories().Resource;
           
            // Filtreleme ve sıralama işlemleri
            query = QueryableExtensions.ApplyFilter(query, queryObject.SortBy, queryObject.FilterBy);
            query = QueryableExtensions.ApplySorting(query, queryObject.SortBy, queryObject.IsDecSending);
            if (query == null)
            {
                return new Response<List<ReadCategoryDto>>($"An error occurred while fetching categories");// Hata mesajı döndür
            }
            // Sayfalama
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
            var categories = await query.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

            return new Response<List<ReadCategoryDto>>(categories);// Başarılı şekilde döndür
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching categories.");
            return new Response<List<ReadCategoryDto>>($"An error occurred while fetching categories: {ex.Message}");// Hata mesajı döndür
        }
    }
    public async Task<Response<ReadCategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto) {
        var isMainCategoryExist = await _unitOfWork.MainCategoryRepository.AnyAsync(createCategoryDto.MainCategoryId);
        if (!isMainCategoryExist)
            return new Response<ReadCategoryDto>("MainCategory does not exist");
        try
        {
            Category category = _mapper.Map<Category>(createCategoryDto);
            category.Slug = SlugHelper.GenerateSlug(category.Name);
          
            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.CategoriesList);
            ReadCategoryDto readCategoryDto = _mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save category.");
            return new Response<ReadCategoryDto>($"An error occurred when saving the category: {ex.Message}");
        }
    }
    public async Task<Response<ReadCategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto) 
    {
        var existingCategory = await _categoryRepository.GetTByIdAsync(id);
        if (existingCategory == null) 
        {
            return new Response<ReadCategoryDto>($"Category with id: {id} not found.");
        }

        try
        {
            _mapper.Map(dto, existingCategory);

            if (!string.IsNullOrEmpty(dto.Name)) 
            {
                existingCategory.Slug = SlugHelper.GenerateSlug(existingCategory.Name);
            }

            _categoryRepository.Update(existingCategory);
        
            await _unitOfWork.CommitAsync();

            _memoryCache.Remove(CacheKeys.CategoriesList);

            var updatedCategoryDto = _mapper.Map<ReadCategoryDto>(existingCategory);
            return new Response<ReadCategoryDto>(updatedCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update Category with ID {id}.", id);
            return new Response<ReadCategoryDto>($"An error occurred when updating the Category: {ex.Message}");
        }
    }
    public async Task<Response<ReadCategoryDto>> DeleteCategoryAsync(int id) {
        
        var existCategory = await _categoryRepository.GetTByIdAsync(id);
        if (existCategory == null) return new Response<ReadCategoryDto>("Category not found.");
        try
        {
            _categoryRepository.Delete(existCategory);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.CategoriesList);
            ReadCategoryDto readCategoryDto = _mapper.Map<ReadCategoryDto>(existCategory);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not delete Category with ID {id}.", id);
            return new Response<ReadCategoryDto>($"An error occurred when deleting the Category: {e.Message}");
        }
    }
    public async Task<Response<bool>> CategoryExistAsync(int id) {
        try
        {
            // Ürünün var olup olmadığını kontrol et
            var exists = await _context.Categories.AnyAsync(e => e.Id == id);

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
            _logger.LogError(ex, "An error occurred while checking if Category exists.");
            return new Response<bool>($"An error occurred while checking if Category exists: {ex.Message}"); // Hata mesajı döndür
        }
    }
   
  
}

