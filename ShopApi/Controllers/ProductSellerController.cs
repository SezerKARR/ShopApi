namespace ShopApi.Controllers;

using Dtos.ProductSeller;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("api/[controller]")]
[ApiController]
public class ProductSellerController : ControllerBase{
    private readonly IProductSellerService _productSellerService;
    public ProductSellerController(IProductSellerService productSellerService) {
        _productSellerService = productSellerService;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadProductSellerDto>> GetProductSellerById(int id,int includes=-1) {
        var response=await _productSellerService.GetProductSellerByIdAsync(id,includes);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<ActionResult<ReadProductSellerDto>> CreateProductSeller(CreateProductSellerDto createProductSellerDto) {
        var response = await _productSellerService.CreateProductSellerAsync(createProductSellerDto);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
}
