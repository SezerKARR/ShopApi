namespace ShopApi.Controllers;

using Dtos.Product;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Models;
using SaplingStore.Helpers;
using Services;

[Route("api/[controller]")]
[ApiController] 
public class ProductController(IProductService productService, ICategoryService categoryService) : Controller {
    [HttpGet]
    public async Task<List<ReadProductDto>> GetAllProductsAsync() {
        Console.WriteLine("asd");
        return await productService.GetProductsAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadProductDto?>> GetProductById(int id) {
        return (await productService.GetProductByIdAsync(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto) {
        Console.WriteLine("Create Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await categoryService.CategoryExistAsync(createProductDto.CategoryId))
            return BadRequest("Category does not exist");


        try
        {
            ReadProductDto product= await productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id },
            product);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, ex + "Internal server error");
        }
        
    }
}