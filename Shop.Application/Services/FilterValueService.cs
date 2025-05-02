namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.FilterValue;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Cache;
using Shop.Infrastructure.Data;

public interface IFilterValueService {
    Task<Response<List<FilterValue>>> GetFilterValuesAsync();
    Task<Response<FilterValue>> CreateFilterValueAsync(CreateFilterValueDto newFilterValue);
    Task<Response<FilterValue>> GetFilterValueById(int filterValueId);

}

public class FilterValueService:IFilterValueService {
    private readonly IFilterValueRepository _filterValueRepository;
    private readonly ILogger<FilterValueService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public FilterValueService(IFilterValueRepository filterValueRepository, ILogger<FilterValueService> logger, IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper) {
        _filterValueRepository = filterValueRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }
    public async Task<Response<List<FilterValue>>> GetFilterValuesAsync() {
        try
        {
            var filterValues = await _memoryCache.GetOrCreateAsync(CacheKeys.FilterValueList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _filterValueRepository.GetAllAsync();
            });
            if (filterValues == null)
            {
                return new Response<List<FilterValue>>("filterValue not found");
            }
            return new Response<List<FilterValue>>(filterValues);
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching filterValue.");
            return new Response<List<FilterValue>>($"An error occurred while fetching filterValue: {ex.Message}");
        }
    }
    public async Task<Response<FilterValue>> CreateFilterValueAsync(CreateFilterValueDto newFilterValue)
    {
        
        try
        {
            var filter = await _unitOfWork.FilterRepository.GetByIdAsync(newFilterValue.FilterId);
            if (filter == null)
                return new Response<FilterValue>("Filter not found");
            if (filter.Type == FilterType.None)
            {
                return new Response<FilterValue>("Filter type not specified");
            }
           
            FilterValue filterValue = _mapper.Map<FilterValue>(newFilterValue);
            filterValue.Slug = SlugHelper.GenerateSlug(newFilterValue.Name);
            await _filterValueRepository.CreateAsync(filterValue);

            if (!await _unitOfWork.CommitAsync())
                return new Response<FilterValue>($"An error occurred when creating filterValue: {newFilterValue.Name}");

            _memoryCache.Remove(CacheKeys.FilterValueList);
            return new Response<FilterValue>(filterValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating filterValue.");
            return new Response<FilterValue>($"An error occurred: {ex.Message}");
        }
    }

    public async Task<Response<FilterValue>> GetFilterValueById(int filterValueId) {
        try
        {
            var filterValue = await _filterValueRepository.GetByIdAsync(filterValueId);
            if (filterValue == null) { return new Response<FilterValue>($"filterValue not found:{filterValueId}"); }
            _memoryCache.Remove(CacheKeys.FilterValueList);
            return new Response<FilterValue>(filterValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating filterValue.");
            return new Response<FilterValue>($"An error occurred when creating filterValue:{ex.Message}");
        }
    }
    
}