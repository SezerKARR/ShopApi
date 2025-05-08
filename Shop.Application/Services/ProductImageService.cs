namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.ProductImage;
using Helpers;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Shop.Infrastructure.Data;

public interface IProductImageService {
    Task<Response<List<ReadProductImageDto>>> GetProductImagesAsync(int includes=-1);
    Task<Response<List<ReadProductImageDto>>> GetProductImagesByProductIdAsync(int productId,int includes=-1);
    Task<Response<ReadProductImageDto>> CreateProductImageAsync(CreateProductImageDto createProductImageDto,bool useTransaction = true);
}

public class ProductImageService:IProductImageService {
    private readonly IProductImageRepository _productImageRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductImage> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public ProductImageService(IProductImageRepository productImageRepository, IMapper mapper,  IUnitOfWork unitOfWork, ILogger<ProductImage> logger, IImageService imageService) {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _imageService = imageService;
    }


    public async Task<Response<List<ReadProductImageDto>>> GetProductImagesAsync(int includes=-1) {
        var productImages = await _productImageRepository.GetAllAsync( includes);
        if (productImages == null)
        {
            return new Response<List<ReadProductImageDto>>("No product Image found");
        }
        List<ReadProductImageDto> readProductImageDto = _mapper.Map<List<ReadProductImageDto>>(productImages);
        return new Response<List<ReadProductImageDto>>(readProductImageDto);
    }
    
    public async Task<Response<List<ReadProductImageDto>>> GetProductImagesByProductIdAsync(int productId,int includes=-1) {
        var productImages=await _productImageRepository.GetProductImagesByProductIdAsync(productId,includes);
        if (productImages == null)
        {
            return new Response<List<ReadProductImageDto>>("No product Image found");
        }
        List<ReadProductImageDto> readProductImageDto = _mapper.Map<List<ReadProductImageDto>>(productImages);
        return new Response<List<ReadProductImageDto>>(readProductImageDto);
    }
    public async Task<Response<ReadProductImageDto>> CreateProductImageAsync(CreateProductImageDto createProductImageDto,bool useTransaction=true) {
        if (useTransaction)
        {
            await _unitOfWork.BeginTransactionAsync();

        }
        var isProductExist = await _unitOfWork.ProductRepository.AnyAsync(createProductImageDto.ProductId);
        if (!isProductExist)
            return new Response<ReadProductImageDto>("product does not exist.");
        var sameProductImages=await _productImageRepository.GetProductImagesByProductIdAsync(createProductImageDto.ProductId,includes:2);
        if (sameProductImages != null && sameProductImages.Any(pi => pi.Order == createProductImageDto.Order))
        {
            var needChangeOrderProductImages = sameProductImages
                .Where(pi => pi.Order >= createProductImageDto.Order)
                .OrderByDescending(pi => pi.Order) // büyük order'lar önce güncellensin
                .ToList();

            foreach (var changeOrderProductImage in needChangeOrderProductImages)
            {
                changeOrderProductImage.Order += 1;
                 _productImageRepository.Update(changeOrderProductImage); // eğer update async ise
                
            }
            
        }
        try
        {
            var productImage = _mapper.Map<ProductImage>(createProductImageDto);
            productImage.Slug= SlugHelper.GenerateSlug(productImage.Name);
          
            await _productImageRepository.CreateAsync(productImage);
            if (!await _unitOfWork.CommitAsync())
            {
                await _unitOfWork.RollbackAsync();
                return new Response<ReadProductImageDto>($"An error occurred when creating product Image: {productImage.Id}");
					
            }
            if (useTransaction)
            {
                await _unitOfWork.CommitTransactionAsync();

            }
            ReadProductImageDto readProductImageDto = _mapper.Map<ReadProductImageDto>(productImage);
            return new Response<ReadProductImageDto>(readProductImageDto);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(e);
            return new Response<ReadProductImageDto>(e.Message);
        }
    }
    

}
