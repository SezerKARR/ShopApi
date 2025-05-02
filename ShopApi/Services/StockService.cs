namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.ProductSeller;
using Dtos.Stock;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using Repository;
using Shared.Cache;

public interface IStockService {
    Task<Response<ReadStockDto>> GetStockById(int id);
    Task<Response<List<ReadStockDto>>> GetAllStocks();
    Task<Response<ReadStockDto>> CreateProductSellerAsync(CreateStockDto createStockDto);
}

public class StockService : IStockService {
    readonly IUnitOfWork _unitOfWork;
    readonly IMemoryCache _memoryCache;
    readonly IStockRepository _stockRepository;
    readonly IMapper _mapper;
    readonly ILogger<Stock> _logger;
    public StockService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper, ILogger<Stock> logger) {
        _unitOfWork = unitOfWork;
        _stockRepository = unitOfWork.StockRepository;
        _memoryCache = memoryCache;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Response<ReadStockDto>> GetStockById(int id) {
        var stock = await _stockRepository.GetByIdAsync(id);
        if (stock == null)
        {
            return new Response<ReadStockDto>("Stock not found");
        }
        var stockDto = _mapper.Map<ReadStockDto>(stock);
        return new Response<ReadStockDto>(stockDto);
    }
    public async Task<Response<List<ReadStockDto>>> GetAllStocks() {
        var stocks = await _stockRepository.GetAllAsync();
        if (stocks.Count == 0)
        {
            return new Response<List<ReadStockDto>>("Stocks not found");
        }
        var stocksDto = _mapper.Map<List<ReadStockDto>>(stocks);
        return new Response<List<ReadStockDto>>(stocksDto);
    }
    public async Task<Response<ReadStockDto>> CreateProductSellerAsync(CreateStockDto createStockDto) {
            try
            {
                var isExistProductSeller = await _unitOfWork.ProductSellerRepository.AnyAsync(createStockDto.ProductSellerId);
                if (!isExistProductSeller )
                    return new Response<ReadStockDto>("productSeller not found.");
                Stock newStock = new Stock()
                {
                    ProductSellerId = createStockDto.ProductSellerId, Quantity = createStockDto.Quantity, DateTime = DateTime.Now,

                };                               
                await _stockRepository.CreateAsync(newStock);
                await _unitOfWork.CommitAsync();
                _memoryCache.Remove(CacheKeys.StockList);
                
                var readStockDto = _mapper.Map<ReadStockDto>(newStock);
                return new Response<ReadStockDto>(readStockDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating stock.");
                return new Response<ReadStockDto>($"An error occurred: {ex.Message}");
            }
    }
}