namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Brand;
using Dtos.BrandCategory;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IBrandCategoryService {
    Task<Response<List<ReadBrandCategoryDto>>> GetAllAsync();
    Task<Response<ReadBrandCategoryDto>> CreateAsync(CreateBrandCategoryDto dto);
    Task<Response<ReadBrandCategoryDto>> GetByIdAsync(int id);
    Task<Response<List<ReadBrandDto>>> GetBrandsByCategoryIdAsync(int categoryId);
}
public class BrandCategoryService : IBrandCategoryService {
    private readonly IBrandCategoryRepository _brandCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BrandCategoryService> _logger;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public BrandCategoryService(
        IBrandCategoryRepository brandCategoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<BrandCategoryService> logger,
        IMapper mapper,
        IMemoryCache memoryCache
    ) {
        _brandCategoryRepository = brandCategoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<Response<List<ReadBrandCategoryDto>>> GetAllAsync() {
        try {
            var brandCategories = await _memoryCache.GetOrCreateAsync(CacheKeys.BrandCategoryList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _brandCategoryRepository.GetAllAsync();
            });

            var result = _mapper.Map<List<ReadBrandCategoryDto>>(brandCategories);
            return new Response<List<ReadBrandCategoryDto>>(result);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error fetching brand categories.");
            return new Response<List<ReadBrandCategoryDto>>($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Response<ReadBrandCategoryDto>> CreateAsync(CreateBrandCategoryDto dto) {
        try {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(dto.BrandId);
            if (brand == null)
                return new Response<ReadBrandCategoryDto>("Brand not found.");

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                return new Response<ReadBrandCategoryDto>("Category not found.");
            var entity = _mapper.Map<BrandCategory>(dto);
            
            await _brandCategoryRepository.CreateAsync(entity);
            
            if (!await _unitOfWork.CommitAsync())
                return new Response<ReadBrandCategoryDto>("Error saving to database.");

            _memoryCache.Remove(CacheKeys.BrandCategoryList);

            var result = _mapper.Map<ReadBrandCategoryDto>(entity);
            return new Response<ReadBrandCategoryDto>(result);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error creating brand category.");
            return new Response<ReadBrandCategoryDto>($"An error occurred: {ex.Message}");
        }
    }
    
    public async Task<Response<ReadBrandCategoryDto>> GetByIdAsync(int id) {
        try {
            var entity = await _brandCategoryRepository.GetByIdAsync(id);
            if (entity == null)
                return new Response<ReadBrandCategoryDto>("BrandCategory not found");

            var result = _mapper.Map<ReadBrandCategoryDto>(entity);
            return new Response<ReadBrandCategoryDto>(result);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error fetching brand category.");
            return new Response<ReadBrandCategoryDto>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<List<ReadBrandDto>>> GetBrandsByCategoryIdAsync(int categoryId) {
        try {
          var brandCategories = await _brandCategoryRepository.GetBrandsByCategoryIdAsync(categoryId);

            var result = _mapper.Map<List<ReadBrandDto>>(brandCategories);
            return new Response<List<ReadBrandDto>>(result);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error fetching brand categories.");
            return new Response<List<ReadBrandDto>>($"An error occurred: {ex.Message}");
        }
    }
}
