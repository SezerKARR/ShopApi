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
    public async Task<ActionResult<List<ReadProductDto>>> GetAllProductsAsync() {
        Console.WriteLine("asd");
       var products = await productService.GetProductsAsync();
       if (products.Success)
       {
           return Ok(products);
       }
       return BadRequest(products.Message);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadProductDto?>> GetProductById(int id) {
        var product = await productService.GetProductByIdAsync(id);
        if (product.Success)
        {
            return Ok(product);
        }
        return BadRequest(product.Message);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto) {
        Console.WriteLine("Create Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var isCategoryExist= await categoryService.CategoryExistAsync(createProductDto.CategoryId);
        if (!isCategoryExist.Success)
            return BadRequest("Category does not exist");


        try
        {
            var product= await productService.CreateProductAsync(createProductDto);
            var a = product.Resource;
            return CreatedAtAction(nameof(GetProductById), new { id = a.Id },
            a);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, ex + "Internal server error");
        }
        
    }
}