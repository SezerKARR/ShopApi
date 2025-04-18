namespace ShopApi.Services {
	using AutoMapper;
	using Data;
	using Dtos.ProductSeller;
	using Dtos.Stock;
	using Microsoft.Extensions.Caching.Memory;
	using Models;
	using Models.Common;
	using Repository;
	using Shared.Cache;

	public interface IProductSellerService {
		Task<Response<List<ReadProductSellerDto>>> GetProductSellersAsync();
		Task<Response<ReadProductSellerDto>> CreateProductSellerAsync(CreateProductSellerDto createProductSellerDto,bool useTransaction=true);
		Task<Response<ReadProductSellerDto>> GetProductSellerByIdAsync(int productSellerId);
	}

	public class ProductSellerService : IProductSellerService {
		private readonly IProductSellerRepository _productSellerRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMemoryCache _memoryCache;
		private readonly IMapper _mapper;
		private readonly ILogger<ProductSellerService> _logger;
		private readonly IStockService _stockService;
		public ProductSellerService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IMapper mapper, ILogger<ProductSellerService> logger, IStockService stockService) {
			_unitOfWork = unitOfWork;
			_memoryCache = memoryCache;
			_mapper = mapper;
			_logger = logger;
			_stockService = stockService;
			_productSellerRepository = unitOfWork.ProductSellerRepository;
		}

		public async Task<Response<List<ReadProductSellerDto>>> GetProductSellersAsync() {
			try
			{
				var productSellers = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductSellerList, entry => {
					entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
					return _productSellerRepository.GetAllAsync();
				});

				if (productSellers == null) { return new Response<List<ReadProductSellerDto>>("Product sellers not found."); }

				var productSellerDto = productSellers.Select(_mapper.Map<ReadProductSellerDto>).ToList();
				return new Response<List<ReadProductSellerDto>>(productSellerDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching product sellers.");
				return new Response<List<ReadProductSellerDto>>($"An error occurred: {ex.Message}");
			}
		}
		public async Task<Response<ReadProductSellerDto>> CreateProductSellerAsync(CreateProductSellerDto createProductSellerDto,bool useTransaction=true) {
			if (useTransaction)
			{
				await _unitOfWork.BeginTransactionAsync();

			}
			try
			{
				// var isProductExist = await _unitOfWork.ProductRepository.AnyAsync(createProductSellerDto.ProductId);
				// if (!isProductExist)
				// 	return new Response<ReadProductSellerDto>("Product not found.");

				var isUserExist = await _unitOfWork.UserRepository.AnyAsync(createProductSellerDto.SellerId);
				if (!isUserExist)
					return new Response<ReadProductSellerDto>("User not found.");

				ProductSeller productSeller = new ProductSeller()
				{
					ProductId = createProductSellerDto.ProductId, SellerId = createProductSellerDto.SellerId,
				};
				await _productSellerRepository.CreateAsync(productSeller);
				if (!await _unitOfWork.CommitAsync())
				{
					await _unitOfWork.RollbackAsync();
					return new Response<ReadProductSellerDto>($"An error occurred when creating product seller: {productSeller.Id}");
					
				}
				CreateStockDto createStockDto = new CreateStockDto()
				{
					ProductSellerId = productSeller.Id, Quantity = createProductSellerDto.Quantity,
				};

				var stockResponse = await _stockService.CreateProductSellerAsync(createStockDto);
				if (!stockResponse.Success)
				{
					if (useTransaction)
					{
						await _unitOfWork.RollbackAsync();
					}
					if (stockResponse.Message != null)
						return new Response<ReadProductSellerDto>(stockResponse.Message);
				}



				if (useTransaction)
				{
					await _unitOfWork.CommitTransactionAsync();

				}
				_memoryCache.Remove(CacheKeys.ProductSellerList);
				var productSellerDto = _mapper.Map<ReadProductSellerDto>(productSeller);
				return new Response<ReadProductSellerDto>(productSellerDto);
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackAsync();
				_logger.LogError(ex, "An error occurred while creating product seller.");
				return new Response<ReadProductSellerDto>($"An error occurred: {ex.Message}");
			}
		}


		public async Task<Response<ReadProductSellerDto>> GetProductSellerByIdAsync(int productSellerId) {
			try
			{
				var productSeller = await _productSellerRepository.GetByIdAsync(productSellerId);
				if (productSeller == null) { return new Response<ReadProductSellerDto>($"Product seller not found: {productSellerId}"); }

				_memoryCache.Remove(CacheKeys.ProductSellerList);
				var productSellerDto = _mapper.Map<ReadProductSellerDto>(productSeller);
				return new Response<ReadProductSellerDto>(productSellerDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving product seller.");
				return new Response<ReadProductSellerDto>($"An error occurred: {ex.Message}");
			}
		}
	}
}