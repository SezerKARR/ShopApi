namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Basket;
using Helpers;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IBasketService {
    Task<Response<List<ReadBasketDto>>> GetBasketsAsync();
    Task<Response<ReadBasketDto?>> GetBasketAsync(CreateBasketDto createBasketDto);
    Task<Response<ReadBasketDto?>> GetBasketByIdAsync(int id);
    Task<Response<ReadBasketDto>> CreateBasketAsync(CreateBasketDto createBasketDto);
}

public class BasketService : IBasketService {
    readonly IMapper _mapper;
    readonly IBasketRepository _basketRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<CategoryService> _logger;
    public BasketService(IMapper mapper, IBasketRepository basketRepository, IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<CategoryService> logger) {
        _mapper = mapper;
        _basketRepository = basketRepository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Response<ReadBasketDto?>> GetBasketAsync(CreateBasketDto createBasketDto) {
        var basket = await _basketRepository.GetBasketByUserIdAsync(createBasketDto.UserId) ?? await _basketRepository.GetBasketByUserIpAsync(createBasketDto.UserIp);

        if (basket == null) { return new Response<ReadBasketDto?>($"Basket with id: {createBasketDto.UserIp} not found."); }
        ReadBasketDto readCategoryDto = _mapper.Map<ReadBasketDto>(basket);
        return new Response<ReadBasketDto?>(readCategoryDto);
    }
    public async Task<Response<ReadBasketDto?>> GetBasketByIdAsync(int id) {
        try
        {
            Basket? basket = await _basketRepository.GetByIdAsync(id);

            if (basket == null) { return new Response<ReadBasketDto?>($"Basket with id: {id} not found."); }
            ReadBasketDto readCategoryDto = _mapper.Map<ReadBasketDto>(basket);
            return new Response<ReadBasketDto?>(readCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Basket by id.");
            return new Response<ReadBasketDto?>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<ReadBasketDto>> CreateBasketAsync(CreateBasketDto createBasketDto) {

        try
        {
            Basket basket = _mapper.Map<Basket>(createBasketDto);
            basket.Slug = SlugHelper.GenerateSlug(basket.Name);

            await _basketRepository.CreateAsync(basket);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.CategoriesList);
            ReadBasketDto readBasketDto = _mapper.Map<ReadBasketDto>(basket);
            return new Response<ReadBasketDto>(readBasketDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save Basket.");
            return new Response<ReadBasketDto>($"An error occurred when saving the Basket: {ex.Message}");
        }
    }
    public async Task<Response<List<ReadBasketDto>>> GetBasketsAsync() {
        try
        {
            List<Basket>? baskets = await _memoryCache.GetOrCreateAsync(CacheKeys.BasketList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _basketRepository.GetAllAsync();
            });
            List<ReadBasketDto> readBasketDtos = _mapper.Map<List<ReadBasketDto>>(baskets);
            return new Response<List<ReadBasketDto>>(readBasketDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching Baskets.");
            return new Response<List<ReadBasketDto>>($"An error occurred while fetching Baskets: {ex.Message}");
        }
    }
}