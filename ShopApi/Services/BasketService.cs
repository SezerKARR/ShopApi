namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Basket;
using Dtos.Category;
using Helpers;
using Interface;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IBasketService {
    Task<Response<ReadBasketDto>> CreateBasketAsync(CreateBasketDto createBasketDto);
}
public class BasketService:IBasketService {
    readonly IMapper _mapper;
    readonly AppDbContext _context;
    readonly IBasketRepository _basketRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<CategoryService> _logger;
    public BasketService(IMapper mapper, AppDbContext context,IBasketRepository basketRepository,IMemoryCache memoryCache,IUnitOfWork unitOfWork,ILogger<CategoryService> logger) {
        _mapper = mapper;
        _context = context;
        _basketRepository = basketRepository;
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _logger = logger;
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
            _logger.LogError(ex, "Could not save category.");
            return new Response<ReadBasketDto>($"An error occurred when saving the category: {ex.Message}");
        }
    }
}

