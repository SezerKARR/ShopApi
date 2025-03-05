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
           return Ok(products.Resource);
       }
       return BadRequest(products.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadProductDto?>> GetProductById([FromRoute] int id) {
        var product = await productService.GetProductByIdAsync(id);
        if (product.Success)
        {
            return Ok(product.Resource);
        }
        return BadRequest(product.Message);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto) {
        Console.WriteLine("Create Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var isCategoryExist= await categoryService.CategoryExistAsync(createProductDto.CategoryId);
        if (!isCategoryExist.Resource)
            return BadRequest("Category does not exist");
        try
        {
            var response= await productService.CreateProductAsync(createProductDto);
            if (response.Success)
            {
                var product = response.Resource;
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id },
                product);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, ex + "Internal server error");
        }
        
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath = path });
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id) {
        Console.WriteLine("Delete Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await productService.DeleteProductAsync(id);
        if (response.Success)
        {
            var product = response.Resource;
            return Ok(product);
        }
        return BadRequest(response.Message);


    }
    
    
}