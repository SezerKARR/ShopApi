namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.MainCategory;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Cache;
using Shop.Infrastructure.Data;

public interface IMainCategoryService {
    Task<Response<List<ReadMainCategory>>> GetMainCategoriesAsync();
    Task<Response<ReadMainCategory>> CreateMainCategory(CreateMainCategory createMainCategory);
    Task<Response<ReadMainCategory>> GetMainCategoryById(int mainCategoryId);
}
public class MainCategoryService:IMainCategoryService {
    readonly IMemoryCache _memoryCache;
    readonly IMapper _mapper;
    readonly ILogger<MainCategoryService> _logger;
    readonly IUnitOfWork _unitOfWork;
    readonly IMainCategoryRepository _mainCategoryRepository;
    public MainCategoryService(IMemoryCache memoryCache,IMapper mapper,ILogger<MainCategoryService> logger,IUnitOfWork unitOfWork) {
        _memoryCache = memoryCache;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;

        _mainCategoryRepository = unitOfWork.MainCategoryRepository;
    }
    public async Task<Response<List<ReadMainCategory>>> GetMainCategoriesAsync() {
        try
        {
            var mainCategories = await _memoryCache.GetOrCreateAsync(CacheKeys.MainCategoryList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _mainCategoryRepository.GetAllAsync();
            });
            if (mainCategories == null)
            {
                return new Response<List<ReadMainCategory>>("MainCategory not found");
            }
            List<ReadMainCategory> mainCategoriesDto = mainCategories.Select(mainCategory => _mapper.Map<ReadMainCategory>(mainCategory)).ToList();
            return new Response<List<ReadMainCategory>>(mainCategoriesDto);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products.");
            return new Response<List<ReadMainCategory>>($"An error occurred while fetching products: {ex.Message}");
        }
    }

    public async Task<Response<ReadMainCategory>> CreateMainCategory(CreateMainCategory createMainCategory) {
        try
        {
            MainCategory mainCategory = _mapper.Map<MainCategory>(createMainCategory);
            mainCategory.Slug = SlugHelper.GenerateSlug(mainCategory.Name);
            await _mainCategoryRepository.CreateAsync(mainCategory);
            if (!await _unitOfWork.CommitAsync()) { return new Response<ReadMainCategory>($"An error occurred when creating MainCategory:{mainCategory.Name}"); }
            ReadMainCategory mainCategoryDto = _mapper.Map<ReadMainCategory>(mainCategory);
            _memoryCache.Remove(CacheKeys.MainCategoryList);
            return new Response<ReadMainCategory>(mainCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating MainCategory.");
            return new Response<ReadMainCategory>($"An error occurred when creating MainCategory:{ex.Message}");
        }
    }
    public async Task<Response<ReadMainCategory>> GetMainCategoryById(int mainCategoryId) {
        try
        {
            var mainCategory = await _mainCategoryRepository.GetByIdAsync(mainCategoryId);
            if (mainCategory == null) { return new Response<ReadMainCategory>($"MainCategory not found:{mainCategoryId}"); }
            ReadMainCategory mainCategoryDto = _mapper.Map<ReadMainCategory>(mainCategory);
            _memoryCache.Remove(CacheKeys.MainCategoryList);
            return new Response<ReadMainCategory>(mainCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating MainCategory.");
            return new Response<ReadMainCategory>($"An error occurred when creating MainCategory:{ex.Message}");
        }
    }

}

