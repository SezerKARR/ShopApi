namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.ProductFilterValue;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IProductFilterValueService {
    Task<Response<List<ProductFilterValue>>> GetProductFilterValuesAsync();
    Task<Response<ProductFilterValue>> CreateProductFilterValueAsync(CreateProductValueDto createMainCategory);
    Task<Response<ProductFilterValue>> GetProductFilterValueById(int productFilterValueId);
}

public class ProductFilterValueService : IProductFilterValueService {
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly IProductFilterValueRepository _productFilterValueRepository;
    readonly ILogger<ProductFilterValue> _logger;
    readonly IMapper _mapper;
    public ProductFilterValueService(IMemoryCache cache, IUnitOfWork unitOfWork, IProductFilterValueRepository productFilterValueRepository, ILogger<ProductFilterValue> logger, IMapper mapper) {
        _memoryCache = cache;
        _unitOfWork = unitOfWork;
        _productFilterValueRepository = productFilterValueRepository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<Response<List<ProductFilterValue>>> GetProductFilterValuesAsync() {
        try
        {
            var productFilterValues = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductFilterValueList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _productFilterValueRepository.GetAllAsync();
            });
            if (productFilterValues == null) { return new Response<List<ProductFilterValue>>("productFilterValue not found"); }
            return new Response<List<ProductFilterValue>>(productFilterValues);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching productFilterValue.");
            return new Response<List<ProductFilterValue>>($"An error occurred while fetching productFilterValue: {ex.Message}");
        }
    }
    public async Task<Response<ProductFilterValue>> CreateProductFilterValueAsync(CreateProductValueDto createProductValueDto) {
        try
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(createProductValueDto.ProductId);
            if (product == null)
                return new Response<ProductFilterValue>("Product not found");

            var filter = await _unitOfWork.FilterRepository.GetByIdAsync(createProductValueDto.FilterId);
            if (filter == null)
                return new Response<ProductFilterValue>("Filter not found");

            if (filter.CategoryId != product.CategoryId)
                return new Response<ProductFilterValue>("Filter category and product category mismatch");
            var productFilterValue = new ProductFilterValue()
            {
                ProductId = product.Id,
                FilterId = createProductValueDto.FilterId,
                CustomValue = createProductValueDto.CustomValue,
            };
            if (createProductValueDto.FilterValueId !=-1)
            {
                var newFilterValue = await _unitOfWork.FilterValueRepository.GetByIdAsync(createProductValueDto.FilterValueId);
                if (newFilterValue == null)
                {
                    return new Response<ProductFilterValue>("Filter value not found");
                    //TODO: When execution reaches this point, respond with "Filter value not found." 
                    // but also inform that if admin approval is granted, the product can be submitted again.
                    // The product should be temporarily stored until then.
                    //TODO:PRODUCT SERVİCE CONTROL THAT.
                }
                productFilterValue.FilterValueId = createProductValueDto.FilterValueId;
                productFilterValue.CustomValue = null;
            }

            await _productFilterValueRepository.CreateAsync(productFilterValue);

            if (!await _unitOfWork.CommitAsync())
                return new Response<ProductFilterValue>($"An error occurred when creating productFilterValue: {productFilterValue.Name}");

            _memoryCache.Remove(CacheKeys.ProductFilterValueList);
            return new Response<ProductFilterValue>(productFilterValue);
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
            var productFilterValue = await _productFilterValueRepository.GetByIdAsync(productFilterValueId);
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