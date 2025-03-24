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
    Task<Response<List<ReadCategoryDto>>> GetCategoriesAsync(QueryObject queryObject);
    Task<Response<ReadCategoryDto?>> GetCategoryByIdAsync(int id);
    Task<Response<ReadCategoryDto>> GetCategoryBySlugAsync(string slug);
    Task<Response<ReadCategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<List<ReadCategoryDto>> GetSubCategories(int categoryId);
    Task<List<Category>> GetParentCategories(int categoryId);
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
    public CategoryService(IMapper mapper, AppDbContext context, ICategoryRepository categoryRepository, IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<CategoryService> logger) {
        _mapper = mapper;
        _context = context;
        _categoryRepository = categoryRepository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Response<IQueryable<ReadCategoryDto>> GetQueryCategories() {
        try
        {
            var queryCategories = _categoryRepository.GetQuery();
            IQueryable<ReadCategoryDto> readCategoryDtosQuery = _mapper.Map<IQueryable<ReadCategoryDto>>(queryCategories);
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
            foreach (var readCategoryDto in readCategoryDtos) { readCategoryDto.SubCategories = await GetSubCategoriesRecursive(readCategoryDto.Id); }
            return new Response<List<ReadCategoryDto>>(readCategoryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Categoryies.");
            return new Response<List<ReadCategoryDto>>($"An error occurred while fetching Categories: {ex.Message}");
        }
    }
    private async Task<List<ReadCategoryDto>> GetSubCategoriesRecursive(int id) {
        var subCategories = await _categoryRepository.GetAsync();
           
        var a=  subCategories.Where(c => c.ParentId == id).ToList();
        
        List<ReadCategoryDto> subCategoryDtos = new List<ReadCategoryDto>();

        foreach (var subCategory in a)
        {
            var subCategoryDto = _mapper.Map<ReadCategoryDto>(subCategory);
            subCategoryDto.SubCategories = await GetSubCategoriesRecursive(subCategory.Id);// Alt kategorilerini al
            subCategoryDtos.Add(subCategoryDto);
        }

        return subCategoryDtos;
    }
    public async Task<Response<ReadCategoryDto?>> GetCategoryByIdAsync(int id) {
        try
        {
            Category? category = await _categoryRepository.GetTByIdAsync(id);

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
        try
        {
            if (createCategoryDto.ParentId != -1)
            {
                bool isParentCategoryExist = await _unitOfWork.CategoryRepository.AnyAsync(createCategoryDto.ParentId);
                if (!isParentCategoryExist)
                    return new Response<ReadCategoryDto>("Parent Category does not exist");
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            category.Slug = SlugHelper.GenerateSlug(category.Name);

            await _categoryRepository.CreateAsync(category);
            await _unitOfWork.CommitAsync();

            await UpdateCategoryTreeAsync(category.Id, createCategoryDto.ParentId);

            _memoryCache.Remove(CacheKeys.CategoriesList);

            var readCategoryDto = _mapper.Map<ReadCategoryDto>(category);
            return new Response<ReadCategoryDto>(readCategoryDto);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error while saving category.");
            return new Response<ReadCategoryDto>($"Database error: {dbEx.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save category.");
            return new Response<ReadCategoryDto>($"An unexpected error occurred: {ex.Message}");
        }
    }

    private async Task UpdateCategoryTreeAsync(int categoryId, int parentId = -1) {
        _context.CategoryTrees.Add(new CategoryTree
        {
            AncestorId = categoryId, DescendantId = categoryId, Depth = 0
        });

        if (parentId != -1)
        {
            var parentRelations = await _context.CategoryTrees
                .Where(ct => ct.DescendantId == parentId)
                .ToListAsync();

            foreach (var relation in parentRelations)
            {
                _context.CategoryTrees.Add(new CategoryTree
                {
                    AncestorId = relation.AncestorId, DescendantId = categoryId, Depth = relation.Depth + 1
                });
            }
        }

        await _unitOfWork.CommitAsync();
    }
    public async Task<List<ReadCategoryDto>> GetSubCategories(int categoryId) {
        var categories = await _context.CategoryTrees
            .Where(ct => ct.AncestorId == categoryId && ct.Depth > 0)
            .Select(ct => ct.Descendant)
            .ToListAsync();
        var readCategoryDtos = _mapper.Map<List<ReadCategoryDto>>(categories);
        return readCategoryDtos;
    }

    public async Task<List<Category>> GetParentCategories(int categoryId) {
        return await _context.CategoryTrees
            .Where(ct => ct.DescendantId == categoryId && ct.Depth > 0)
            .Select(ct => ct.Ancestor)
            .ToListAsync();
    }
    public async Task<Response<ReadCategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto) {
        var existingCategory = await _categoryRepository.GetTByIdAsync(id);
        if (existingCategory == null) { return new Response<ReadCategoryDto>($"Category with id: {id} not found."); }

        try
        {
            _mapper.Map(dto, existingCategory);

            if (!string.IsNullOrEmpty(dto.Name)) { existingCategory.Slug = SlugHelper.GenerateSlug(existingCategory.Name); }

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
                return new Response<bool>(true);// Ürün bulunduysa başarılı cevap döndür
            }
            else
            {
                return new Response<bool>(false);// Ürün bulunmadıysa başarısız cevap döndür
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking if Category exists.");
            return new Response<bool>($"An error occurred while checking if Category exists: {ex.Message}");// Hata mesajı döndür
        }
    }
    // public async Task<Response<List<ReadCategoryDto>>> GetSubcategoriesByMainCategory(int id) {
    //     try
    //     {
    //         var subCategory = await _context.Categories
    //             .Where(c => c.ParentId == id)
    //             
    //             .ToListAsync();;
    //         
    //         if (subCategory.Count == 0) { return new Response<List<ReadCategoryDto>>($"Category with  not found."); }
    //         List<ReadCategoryDto> readCategoryDto = _mapper.Map<List<ReadCategoryDto>>(subCategory);
    //         return new Response<List<ReadCategoryDto>>(readCategoryDto);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "An error occurred while fetching Category by slug.");
    //         return new  Response<List<ReadCategoryDto>>($"An error occurred: {ex.Message}");
    //     }
    // }


}