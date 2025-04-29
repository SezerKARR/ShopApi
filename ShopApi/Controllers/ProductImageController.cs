namespace ShopApi.Controllers;

using Dtos.ProductImage;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("api/[controller]")]
[ApiController]
public class ProductImageController:ControllerBase {
    private readonly IProductImageService _productImageService;
    public ProductImageController(IProductImageService productImageService) {
        _productImageService = productImageService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReadProductImageDto>>> GetProductImages(int includes=-1) {
        var response = await _productImageService.GetProductImagesAsync(includes);
        if(!response.Success) return BadRequest(response.Message);
        return Ok(response.Resource);
    }
    [HttpGet("{productId}")]
    public async Task<ActionResult<List<ReadProductImageDto>>> GetProductImagesByProductId(int productId) {
        var response = await _productImageService.GetProductImagesByProductIdAsync(productId);
        if(!response.Success) return BadRequest(response.Message);
        return Ok(response.Resource);
    }
    [HttpPost]
    public async Task<ActionResult<List<ReadProductImageDto>>> CreateProductImages(CreateProductImageDto createProductImageDto) {
        var response = await _productImageService.CreateProductImageAsync(createProductImageDto);
        if(!response.Success) return BadRequest(response.Message);
        return Ok(response.Resource);
    }
    
    

}
