namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.BasketItem;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Shared.Cache;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

public interface IBasketItemService {
    Task<Response<List<ReadBasketItemDto>>> GetBasketItemsByBasketIdAsync(int basketId);
    Task<Response<ReadBasketItemDto>> DeleteBasketItemByIdAsync(int id);
    Task<Response<ReadBasketItemDto>> CreateBasketAsync(CreateBasketItemDto createBasketDto);
    Task<Response<ReadBasketItemDto>> UpdateBasketItemQuantityAsync(int id,int quantity);
}
public class BasketItemService:IBasketItemService {
    readonly IMapper _mapper;
    readonly IBasketItemRepository _basketItemRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<CategoryService> _logger;
    public BasketItemService(IMapper mapper, IBasketItemRepository basketItemRepository,IMemoryCache memoryCache,IUnitOfWork unitOfWork,ILogger<CategoryService> logger) {
        _mapper = mapper;
        _basketItemRepository = basketItemRepository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Response<List<ReadBasketItemDto>>> GetBasketItemsByBasketIdAsync(int basketId) {
        try
        {
            List<BasketItem> basketItems = await _basketItemRepository.GetBasketItemsByBasketId(basketId);
            List<ReadBasketItemDto> readBasketItemDtos = _mapper.Map<List<ReadBasketItemDto>>(basketItems);
            return new Response<List<ReadBasketItemDto>>(readBasketItemDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Basket Items.");
            return new Response<List<ReadBasketItemDto>>($"An error occurred while fetching Basket Items: {ex.Message}");
        }
    }
    public async Task<Response<ReadBasketItemDto>> DeleteBasketItemByIdAsync(int id) {
        var existBasketItem = await _basketItemRepository.GetByIdAsync(id);
        if(existBasketItem == null) return new Response<ReadBasketItemDto>("Basket Item Not Found");
        try
        {
            _basketItemRepository.Delete(existBasketItem);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.BasketItemList);
            ReadBasketItemDto readBasketItemDto = _mapper.Map<ReadBasketItemDto>(existBasketItem);
            return new Response<ReadBasketItemDto>(readBasketItemDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not delete Basket Item with ID {id}.", id);
            return new Response<ReadBasketItemDto>($"An error occurred when deleting the Basket Item: {e.Message}");
        }
    }
   
    public async Task<Response<ReadBasketItemDto>> UpdateBasketItemQuantityAsync(int id, int quantity) {
        var existBasketItem = await _basketItemRepository.GetByIdAsync(id);
        if(existBasketItem == null) return new Response<ReadBasketItemDto>("Basket Item Not Found");
        try
        {
            existBasketItem.Quantity += quantity;
            _basketItemRepository.Update(existBasketItem);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.BasketItemList);
            ReadBasketItemDto readBasketItemDto = _mapper.Map<ReadBasketItemDto>(existBasketItem);
            return new Response<ReadBasketItemDto>(readBasketItemDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not Decrease Basket Item with ID {id}.", id);
            return new Response<ReadBasketItemDto>($"An error occurred when Decrease the Basket Item: {e.Message}");
        }
    }
    public async Task<Response<ReadBasketItemDto>> CreateBasketAsync(CreateBasketItemDto createBasketDto) {
        try
        {
            BasketItem basketItem = _mapper.Map<BasketItem>(createBasketDto);
            basketItem.Slug = SlugHelper.GenerateSlug(basketItem.Name);
          
            await _basketItemRepository.CreateAsync(basketItem);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.BasketItemList);
            ReadBasketItemDto readBasketItem = _mapper.Map<ReadBasketItemDto>(basketItem);
            return new Response<ReadBasketItemDto>(readBasketItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save BasketItem.");
            return new Response<ReadBasketItemDto>($"An error occurred when saving the BasketItem: {ex.Message}");
        }
    }
    
}

