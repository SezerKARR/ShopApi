namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Product;
using Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Models.Common;
using SaplingStore.Helpers;
using Shared.Cache;
using QueryableExtensions=QueryableExtensions;

public class ProductService : IProductService {
    readonly IMapper _mapper;
    readonly IProductRepository _productRepository;
    readonly IMemoryCache _memoryCache;
    readonly IUnitOfWork _unitOfWork;
    readonly ILogger<ProductService> _logger;
    public ProductService(IMapper mapper, IMemoryCache memoryCache, IUnitOfWork unitOfWork, ILogger<ProductService> logger) {
        _mapper = mapper;
       
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _productRepository = _unitOfWork.ProductRepository;
        _logger = logger;
    }


    public async Task<Response<List<Product>>> GetProductsAsync() {
        try
        {
            var products = await _memoryCache.GetOrCreateAsync(CacheKeys.ProductsList, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _productRepository.GetAsync();
            });

            return new Response<List<Product>>(products ?? new List<Product>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products.");
            return new Response<List<Product>>($"An error occurred while fetching products: {ex.Message}");
        }
    }

    public Response<IQueryable<Product>> GetProductsQueryable() {
        try
        {
            var queryProducts = _productRepository.GetQuery();
            return new Response<IQueryable<Product>>(queryProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product query.");
            return new Response<IQueryable<Product>>($"An error occurred while fetching product query: {ex.Message}");
        }
    }
    public async Task<Response<Product>> GetProductByIdAsync(int id) {
        try
        {
            // Ürün listesini Response ile alıyoruz
           var product = await _productRepository.GetTByIdAsync(id);
           if (product == null) { return new Response<Product>($"Product with id: {id} not found."); }
            return new Response<Product>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product by id.");
            return new Response<Product>($"An error occurred: {ex.Message}");
        }
    }


    public async Task<Response<Product>> GetProductBySlugAsync(string slug) {
        try
        {
            // Ürün listesini Response ile alıyoruz
           var product = await _productRepository.GetTBySlugAsync(slug);
           

            if (product == null) { return new Response<Product>($"Product with slug: {slug} not found."); }

            return new Response<Product>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product by slug.");
            return new Response<Product>($"An error occurred: {ex.Message}");
        }
    }
    public async Task<Response<List<Product>>> GetProductsAsync(QueryObject queryObject) {
        try
        {
            var query = GetProductsQueryable().Resource;

            // Filtreleme ve sıralama işlemleri
            query = QueryableExtensions.ApplyFilter(query, queryObject.SortBy, queryObject.FilterBy);
            query = QueryableExtensions.ApplySorting(query, queryObject.SortBy, queryObject.IsDecSending);

            // Sayfalama
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
            var products = await query.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

            return new Response<List<Product>>(products);// Başarılı şekilde döndür
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products.");
            return new Response<List<Product>>($"An error occurred while fetching products: {ex.Message}");// Hata mesajı döndür
        }
    }

    public async Task<Response<Product>> CreateProductAsync(CreateProductDto createProductDto) {
        var isCategoryExist= await _unitOfWork.CategoryRepository.GetTByIdAsync(createProductDto.CategoryId);
        if (isCategoryExist==null)
            return new Response<Product>("Category does not exist");
        try
        {
            Product product = _mapper.Map<Product>(createProductDto);
            product.Slug = SlugHelper.GenerateSlug(product.Name);
            product.ImageUrl=FormManager.Save(createProductDto.ImageFile,"uploads/products",FormTypes.Image);
            await AdjustEntity(product);
            await _productRepository.CreateAsync(product);
            if (!await _unitOfWork.CommitAsync())
            {
                return new Response<Product>($"An error occurred when creating product: {product.Name}.");
            }
            
            _memoryCache.Remove(CacheKeys.ProductsList);
            return new Response<Product>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save product.");
            return new Response<Product>($"An error occurred when saving the product: {ex.Message}");
        }


    }
    private async Task AdjustEntity(Product entity) {
        try
        {
        }
        catch (Exception ex)
        {
            // Exception'ı loglayalım
            _logger.LogWarning(ex, "Error loading category for product {ProductName} with category ID {CategoryId}", 
            entity.Name, entity.CategoryId);
        
            // Ya da exception'ı yukarı fırlatabiliriz
            // throw;
        }
    }
    public async Task<Response<Product>> UpdateProductAsync(int id, UpdateProductDto dto) {
        var existingProduct = await GetProductByIdAsync(id);
        Product product = existingProduct.Resource;
        if (product == null) return new Response<Product>($"Product with id: {id} does not exist.");
        try
        {
            _mapper.Map(dto, product);
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.ProductsList);
            return new Response<Product>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update Product with ID {id}.", id);
            return new Response<Product>($"An error occurred when updating the Product: {ex.Message}");
        }
    }
    public async Task<Response<Product>> DeleteProductAsync(int id) {
        var existProduct = await _productRepository.GetTByIdAsync(id);
        if (existProduct == null) return new Response<Product>("Product not found.");
        try
        {
            _productRepository.Delete(existProduct);
            await _unitOfWork.CommitAsync();
            _memoryCache.Remove(CacheKeys.ProductsList);
            return new Response<Product>(existProduct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not delete Product with ID {id}.", id);
            return new Response<Product>($"An error occurred when deleting the Product: {e.Message}");
        }
    }
    public async Task<Response<bool>> ProductExistAsync(int id)
    {
        try
        {
            // Ürünün var olup olmadığını kontrol et
            var exists = await _productRepository.GetTByIdAsync(id);

            if (exists!=null)
            {
                return new Response<bool>(true); // Ürün bulunduysa başarılı cevap döndür
            }
            else
            {
                return new Response<bool>(false); // Ürün bulunmadıysa başarısız cevap döndür
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking if product exists.");
            return new Response<bool>($"An error occurred while checking if product exists: {ex.Message}"); // Hata mesajı döndür
        }
    }


}