namespace ShopApi.Services;

using Data;
using Helpers;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IProductFilterValueService {
    Task<Response<List<ProductFilterValue>>> GetProductFilterValuesAsync();
    Task<Response<ProductFilterValue>> CreateProductFilterValueAsync(ProductFilterValue createMainCategory);
    Task<Response<ProductFilterValue>> GetProductFilterValueById(int productFilterValueId);
}

public class ProductFilterValueService : IProductFilterValueService {
    readonly IMemoryCache  _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly IProductFilterValueRepository _productFilterValueRepository;
    readonly ILogger<ProductFilterValue> _logger;
    public ProductFilterValueService(IMemoryCache  cache,IUnitOfWork unitOfWork,IProductFilterValueRepository productFilterValueRepository, ILogger<ProductFilterValue> logger) {
        _memoryCache = cache;
        _unitOfWork = unitOfWork;
        _productFilterValueRepository = productFilterValueRepository;
        _logger = logger;
    }
    public async Task<Response<List<ProductFilterValue>>> GetProductFilterValuesAsync() {
        try
        {
            var productFilterValues = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductFilterValueList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _productFilterValueRepository.GetAllAsync();
            });
            if (productFilterValues == null)
            {
                return new Response<List<ProductFilterValue>>("productFilterValue not found");
            }
            return new Response<List<ProductFilterValue>>(productFilterValues);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching productFilterValue.");
            return new Response<List<ProductFilterValue>>($"An error occurred while fetching productFilterValue: {ex.Message}");
        }
    }
    public async Task<Response<ProductFilterValue>> CreateProductFilterValueAsync(ProductFilterValue newFilterValue)
    {
        try
        {
            var product = await _unitOfWork.ProductRepository.GetTByIdAsync(newFilterValue.ProductId);
            if (product == null)
                return new Response<ProductFilterValue>("Product not found");

            var filter = await _unitOfWork.FilterRepository.GetTByIdAsync(newFilterValue.CategoryFilterId);
            if (filter == null)
                return new Response<ProductFilterValue>("Filter not found");

            if (filter.CategoryId != product.CategoryId)
                return new Response<ProductFilterValue>("Filter category and product category mismatch");

            newFilterValue.Slug = SlugHelper.GenerateSlug(newFilterValue.Name);
            await _productFilterValueRepository.CreateAsync(newFilterValue);

            if (!await _unitOfWork.CommitAsync())
                return new Response<ProductFilterValue>($"An error occurred when creating productFilterValue: {newFilterValue.Name}");

            _memoryCache.Remove(CacheKeys.ProductFilterValueList);
            return new Response<ProductFilterValue>(newFilterValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating productFilterValue.");
            return new Response<ProductFilterValue>($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Response<ProductFilterValue>> GetProductFilterValueById(int productFilterValueId) {
        try
        {
            var productFilterValue = await _productFilterValueRepository.GetTByIdAsync(productFilterValueId);
            if (productFilterValue == null) { return new Response<ProductFilterValue>($"productFilterValue not found:{productFilterValueId}"); }
            _memoryCache.Remove(CacheKeys.ProductFilterValueList);
            return new Response<ProductFilterValue>(productFilterValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating productFilterValue.");
            return new Response<ProductFilterValue>($"An error occurred when creating productFilterValue:{ex.Message}");
        }
    }
}