namespace ShopApi.Services;

using AutoMapper;
using Data;
using Dtos.Product;
using Dtos.ProductImage;
using Helpers;
using Models;
using Models.Common;
using Repository;

public class ProductImageService {
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly Logger<ProductImage> _logger;
    private readonly UnitOfWork _unitOfWork;
    
    public ProductImageService(IProductImageRepository productImageRepository, IMapper mapper, Logger<ProductImage> logger, UnitOfWork unitOfWork) {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
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
    public async Task<Response<ReadProductImageDto>> CreateProductImageAsync(CreateProductImageDto createProductImageDto) {
        var isProductExist = await _unitOfWork.ProductRepository.AnyAsync(createProductImageDto.ProductId);
        if (!isProductExist)
            return new Response<ReadProductImageDto>("product does not exist.");
        try
        {
            var productImage = _mapper.Map<ProductImage>(createProductImageDto);
            productImage.Slug= SlugHelper.GenerateSlug(productImage.Name);
            productImage.Url=FormManager.Save(createProductImageDto.Image, "uploads/productImage", FormTypes.Image);
            await _productImageRepository.CreateAsync(productImage);
            await _unitOfWork.CommitAsync();
            ReadProductImageDto readProductImageDto = _mapper.Map<ReadProductImageDto>(productImage);
            return new Response<ReadProductImageDto>(readProductImageDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<ReadProductImageDto>(e.Message);
        }
    }
    

}
