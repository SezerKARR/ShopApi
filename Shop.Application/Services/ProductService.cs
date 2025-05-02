namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Domain.Models.Request;
using Dtos.Product;
using Dtos.ProductSeller;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Cache;
using Shop.Infrastructure.Data;

public interface IProductService {
    Task<Response<List<ReadProductDto>>> GetProductsAsync(int includes = -1);
    Task<Response<List<ReadProductDto>>> GetFilteredProducts(ProductFilterRequest productFilterRequest, int includes = -1);

    Task<Response<List<ReadProductDto>>> GetProductByCategoryIdAsync(int categoryId, int includes = -1);
    Task<Response<ReadProductDto>> GetProductByIdAsync(int id, int includes = -1);
    Task<Response<ReadProductDto>> CreateProductAsync(CreateProductDto createProductDto);
    Task<Response<ReadProductDto>> DeleteProductAsync(int id, int includes = -1);

    // Task<Response<List<ReadProductDto>>> GetProductsAsync(QueryObject queryObject);
    // Response<IQueryable<ReadProductDto>> GetProductsQueryable();
    // Task<Response<ReadProductDto>> GetProductBySlugAsync(string slug);
    // Task<Response<ReadProductDto>> UpdateProductAsync(int id, UpdateProductDto dto);
    // Task<Response<bool>> ProductExistAsync(int id);

}

public class ProductService:IProductService {
    readonly IMapper _mapper;
    readonly IProductRepository _productRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<ProductService> _logger;
    readonly IProductFilterValueService _productFilterValueService;
    readonly IProductSellerService _productSellerService;
    readonly IProductImageService _productImageService;

    public ProductService(IMapper mapper, IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<ProductService> logger, IProductFilterValueService productFilterValueService, IProductSellerService productSellerService, IProductImageService productImageService) {
        _mapper = mapper;

        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _productRepository = _unitOfWork.ProductRepository;
        _logger = logger;
        _productFilterValueService = productFilterValueService;
        _productSellerService = productSellerService;
        _productImageService = productImageService;
    }


    public async Task<Response<List<ReadProductDto>>> GetProductsAsync(int includes = -1) {
        try
        {
            var products = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductsList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _productRepository.GetAllAsync(includes);
            });
            if (products == null) { return new Response<List<ReadProductDto>>("products not found"); }
            List<ReadProductDto> productDto = products.Select(product => _mapper.Map<ReadProductDto>(product)).ToList();
            return new Response<List<ReadProductDto>>(productDto);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products.");
            return new Response<List<ReadProductDto>>($"An error occurred while fetching products: {ex.Message}");
        }
    }

    // public Response<IQueryable<ReadProductDto>> GetProductsQueryable() {
    // 	try
    // 	{
    // 		var queryProducts = _productRepository.GetQuery();
    // 		if (queryProducts == null) { return new Response<IQueryable<ReadProductDto>>("products not found"); }
    // 		IQueryable<ReadProductDto> readProductDtos = queryProducts.Select(product => _mapper.Map<ReadProductDto>(product));
    // 		return new Response<IQueryable<ReadProductDto>>(readProductDtos);
    // 	}
    // 	catch (Exception ex)
    // 	{
    // 		_logger.LogError(ex, "An error occurred while fetching product query.");
    // 		return new Response<IQueryable<ReadProductDto>>($"An error occurred while fetching product query: {ex.Message}");
    // 	}
    // }
    public async Task<Response<ReadProductDto>> GetProductByIdAsync(int id, int includes = -1) {
        try
        {
            var product = await _productRepository.GetByIdAsync(id, includes);
            if (product == null) { return new Response<ReadProductDto>($"Product with id: {id} not found."); }
            ReadProductDto productDto = _mapper.Map<ReadProductDto>(product);
            return new Response<ReadProductDto>(productDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product by id.");
            return new Response<ReadProductDto>($"An error occurred: {ex.Message}");
        }
    }


    // public async Task<Response<ReadProductDto>> GetProductBySlugAsync(string slug, int includes = -1) {
    //     try
    //     {
    //         var product = await _productRepository.GetBySlugAsync(slug);
    //
    //
    //         if (product == null) { return new Response<ReadProductDto>($"Product with slug: {slug} not found."); }
    //         ReadProductDto productDto = _mapper.Map<ReadProductDto>(product);
    //         return new Response<ReadProductDto>(productDto);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "An error occurred while fetching product by slug.");
    //         return new Response<ReadProductDto>($"An error occurred: {ex.Message}");
    //     }
    // }
    // public async Task<Response<List<ReadProductDto>>> GetProductsAsync(QueryObject queryObject) {
    // 	var query = _productRepository.GetQuery();
    //
    // 	query = QueryableExtensions.ApplyFilter(query, queryObject.SortBy, queryObject.FilterBy);
    // 	query = QueryableExtensions.ApplySorting(query, queryObject.SortBy, queryObject.IsDescending);
    // 	if (query == null)
    // 		return new Response<List<ReadProductDto>>("No products found.");
    // 	var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
    //
    // 	var productDtos = await query
    // 		.Skip(skipNumber)
    // 		.Take(queryObject.PageSize)
    // 		.Select(product => _mapper.Map<ReadProductDto>(product))
    // 		.ToListAsync();
    //
    // 	return productDtos.Any()
    // 		? new Response<List<ReadProductDto>>(productDtos)
    // 		: new Response<List<ReadProductDto>>("No products found.");
    // }


    public async Task<Response<ReadProductDto>> CreateProductAsync(CreateProductDto dto) {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var isUserExist = await _unitOfWork.UserRepository.AnyAsync(dto.CreatedByUserId);
            if (!isUserExist)
                return new Response<ReadProductDto>("User does not exist.");

            var isCategoryExist = await _unitOfWork.CategoryRepository.AnyAsync(dto.CategoryId);
            if (!isCategoryExist)
                return new Response<ReadProductDto>("Category does not exist.");

            var isBrandExist = await _unitOfWork.BrandRepository.AnyAsync(dto.BrandId);
            if (!isBrandExist)
                return new Response<ReadProductDto>("Brand does not exist.");


            var product = new Product
            {
                Name = dto.Name,
                Slug = SlugHelper.GenerateSlug(dto.Name),
                CategoryId = dto.CategoryId,
                BrandId = dto.BrandId,
                MinPrice = dto.Price,
                CreatedBySellerId = dto.CreatedByUserId,
            };

            await _productRepository.CreateAsync(product);
            if (!await _unitOfWork.CommitAsync())
            {
                await _unitOfWork.RollbackAsync();
                return new Response<ReadProductDto>("An error occurred when creating a new product.");
            }
            if (dto.ProductFilterValues != null)
            {
                foreach (var productFilterValue in dto.ProductFilterValues)
                {
                    productFilterValue.ProductId = product.Id;
                    var response = await _productFilterValueService.CreateProductFilterValueAsync(productFilterValue);
                    if (!response.Success)
                    {
                        await _unitOfWork.RollbackAsync();
                        if (response.Message != null) return new Response<ReadProductDto>(response.Message);
                    }
                    // productFilterValues.Add(newProductFilterValue);
                    // await _unitOfWork.ProductFilterValueRepository.CreateAsync(newProductFilterValue);
                }
            }

            var createProductSellerDto = new CreateProductSellerDto()
            {
                ProductId = product.Id, SellerId = product.CreatedBySellerId, Quantity = dto.Quantity, Price = dto.Price
            };
            if (dto.ProductImages != null)
            {
                foreach (var productImage in dto.ProductImages)
                {
                    productImage.ProductId = product.Id;
                    var productImageResponse = await _productImageService.CreateProductImageAsync(productImage,false);
                    if (!productImageResponse.Success)
                    {
                        await _unitOfWork.RollbackAsync();
                        if (productImageResponse.Message != null) return new Response<ReadProductDto>(productImageResponse.Message);
                    }
                }
            }
            var productSellerResponse = await _productSellerService.CreateProductSellerAsync(createProductSellerDto, false);
            if (!productSellerResponse.Success)
            {
                await _unitOfWork.RollbackAsync();
                if (productSellerResponse.Message != null) return new Response<ReadProductDto>(productSellerResponse.Message);
            }
            await _unitOfWork.CommitTransactionAsync();
            Console.WriteLine($"Successfully created a new product seller: {productSellerResponse.Message}");
            var productDto = _mapper.Map<ReadProductDto>(product);
            return new Response<ReadProductDto>(productDto);
        }
        catch (Exception ex)
        {

            try { await _unitOfWork.RollbackAsync(); }
            catch (Exception rollbackEx) { Console.WriteLine($"Rollback failed: {rollbackEx.Message}"); }

            Console.WriteLine($"Main Error: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"Inner: {ex.InnerException.Message}");

            return new Response<ReadProductDto>($"Error: {ex.Message}");

        }
    }


    // private Task AdjustEntity(Product entity) {
    // 	try { }
    // 	catch (Exception ex)
    // 	{
    // 		_logger.LogWarning(ex, "Error loading category for product {ProductName} with category ID {CategoryId}",
    // 		entity.Name, entity.CategoryId);
    // 	}
    // 	return Task.CompletedTask;
    // }
    public async Task<Response<ReadProductDto>> UpdateProductAsync(int id, UpdateProductDto dto, int includes = -1) {
        var existingProduct = await _productRepository.GetByIdAsync(id, includes);
        if (existingProduct == null) return new Response<ReadProductDto>($"Product with id: {id} does not exist.");
        try
        {
            _mapper.Map(dto, existingProduct);
            _productRepository.Update(existingProduct);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.ProductsList);
            ReadProductDto productDto = _mapper.Map<ReadProductDto>(existingProduct);
            return new Response<ReadProductDto>(productDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update Product with ID {id}.", id);
            return new Response<ReadProductDto>($"An error occurred when updating the Product: {ex.Message}");
        }
    }
    public async Task<Response<ReadProductDto>> DeleteProductAsync(int id, int includes = -1) {
        var existProduct = await _productRepository.GetByIdAsync(id, includes);
        if (existProduct == null) return new Response<ReadProductDto>("Product not found.");
        try
        {
            _productRepository.Delete(existProduct);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.ProductsList);
            ReadProductDto productDto = _mapper.Map<ReadProductDto>(existProduct);
            return new Response<ReadProductDto>(productDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not delete Product with ID {id}.", id);
            return new Response<ReadProductDto>($"An error occurred when deleting the Product: {e.Message}");
        }
    }
    public async Task<Response<bool>> ProductExistAsync(int id, int includes = -1) {
        try
        {
            var exists = await _productRepository.GetByIdAsync(id, includes);

            if (exists != null) { return new Response<bool>(true); }
            else { return new Response<bool>(false); }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking if product exists.");
            return new Response<bool>($"An error occurred while checking if product exists: {ex.Message}");
        }
    }

    public async Task<Response<List<ReadProductDto>>> GetFilteredProducts(ProductFilterRequest productFilterRequest, int includes = -1) {
        try
        {
            var products = await _productRepository.GetFilteredProducts(productFilterRequest, includes);

            List<ReadProductDto> productDto = _mapper.Map<List<ReadProductDto>>(products);

            return new Response<List<ReadProductDto>>(productDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<List<ReadProductDto>>("An error occurred while getting products." + e.Message);
        }

    }

    public async Task<Response<List<ReadProductDto>>> GetProductByCategoryIdAsync(int categoryId, int includes = -1) {
        try
        {
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId, includes);
            if (products == null || !products.Any()) return new Response<List<ReadProductDto>>("Product not found.");
            List<ReadProductDto> productDto = _mapper.Map<List<ReadProductDto>>(products);
            return new Response<List<ReadProductDto>>(productDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<List<ReadProductDto>>("Product not found." + e.Message);
        }
    }

}