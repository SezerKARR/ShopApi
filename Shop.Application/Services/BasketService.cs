namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.Basket;
using Dtos.BasketItem;
using Dtos.Product;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Cache;
using Shop.Infrastructure.Data;

public interface IBasketService {
    Task<Response<List<ReadBasketDto>>> GetBasketsAsync(int includes=-1);
    Task<Response<ReadBasketDto?>> GetBasketByUserIdAsync(int userId,int includes=-1);
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
    public async Task<Response<ReadBasketDto?>> GetBasketByUserIdAsync(int userId,int includes=-1) {
        var basket = await _basketRepository.GetBasketByUserIdAsync(userId, includes);

        if (basket == null) { return new Response<ReadBasketDto?>($"Basket with id: {userId} not found."); }
        ReadBasketDto readBasketDto = MapToReadBasket(basket);
        return new Response<ReadBasketDto?>(readBasketDto);
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

    ReadBasketDto MapToReadBasket(Basket? basket)
    {
        // 1. Null Giriş Kontrolü
        if (basket == null)
        {
            Console.WriteLine("[WARN] MapToReadBasket received a null Basket object."); // Önemli bir durum, log kalabilir.
            return new ReadBasketDto { SellerGroups = new List<ReadGroupedBasketItemsDto>() }; // Boş DTO
        }

        ReadBasketDto readBasketDto;

        try
        {
            readBasketDto = _mapper.Map<ReadBasketDto>(basket);
            readBasketDto.SellerGroups = new List<ReadGroupedBasketItemsDto>();
            readBasketDto.GrandTotal = 0;
        }
        catch (Exception ex) // AutoMapper veya başka bir ilk hata
        {
            Console.WriteLine($"[ERROR] Initial mapping failed for Basket ID {basket.Id}. Exception: {ex.Message}");
            // Hata durumunda temel bilgilerle (varsa) ama boş gruplarla dön
            return new ReadBasketDto { Id = basket.Id, UserId = basket.UserId, SellerGroups = new List<ReadGroupedBasketItemsDto>() };
        }

        if (basket.BasketItems == null || !basket.BasketItems.Any())
        {
            return readBasketDto; 
        }

        try
        {
           int totalItems = basket.BasketItems.Count();
            var groupedSellerItems = basket.BasketItems
                .Where(item => item.ProductSeller is { Seller: not null, Product: not null })
                .GroupBy(item => item.ProductSeller?.SellerId) 
                .Select(sellerGroup =>
                {
                    var itemsDtoList = sellerGroup.Select(item => new ReadBasketItemDto
                    {
                        Id = item.Id,
                        Product = _mapper.Map<ReadProductDto>(item.ProductSeller?.Product),
                        Quantity = item.Quantity,
                        Price = item.ProductSeller?.Price ?? -1,
                        ProductSellerId = item.ProductSeller?.Id??-1,
                        // ProductImage = item.ProductSeller.Product.ProductImages.FirstOrDefault()?.Image

                    }).ToList();
                    decimal subtotal = itemsDtoList.Sum(i => i.TotalPrice);
                    bool isSingleItemAndProductHasFreeShipping = 
                        itemsDtoList is [{ Product.IsShippingFree: true }];


                    bool isShippingFree = 
                        isSingleItemAndProductHasFreeShipping ||
                        sellerGroup.First().ProductSeller?.Seller?.FreeShippingMinimumOrderAmount <= subtotal;

                    return new ReadGroupedBasketItemsDto
                    {
                        SellerId = sellerGroup.Key??-1,
                        SellerName = sellerGroup.First().ProductSeller?.Seller?.Name ??"-1", // İlk öğeden al (Where garantisiyle)
                        Items = itemsDtoList,
                        Subtotal = subtotal ,
                        FreeShippingMinimumOrderAmount=sellerGroup.First().ProductSeller?.Seller?.FreeShippingMinimumOrderAmount??-1,
                        IsShippingFree = isShippingFree
                        
                    };
                })
                .ToList(); 
            if (groupedSellerItems.Any()) 
            {
                readBasketDto.SellerGroups = groupedSellerItems;
                readBasketDto.TotalProductAmount = totalItems;
                readBasketDto.GrandTotal = groupedSellerItems.Sum(g => g.Subtotal); // Genel toplamı hesapla
            }
           
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"[ERROR] Grouping/Mapping failed for Basket ID {basket.Id}. Check Includes. Exception: {ex.Message}");
            readBasketDto.SellerGroups = new List<ReadGroupedBasketItemsDto>();
            readBasketDto.GrandTotal = 0;
        }

        return readBasketDto;
    }

    public async Task<Response<List<ReadBasketDto>>> GetBasketsAsync(int includes=-1) {
        try
        {
            List<Basket>? baskets = await _memoryCache.GetOrCreateAsync(CacheKeys.BasketList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _basketRepository.GetAllAsync(3);
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