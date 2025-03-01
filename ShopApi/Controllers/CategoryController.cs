namespace ShopApi.Controllers;

using Dtos.Category;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("api/[controller]")]
[ApiController] 
public class CategoryController(ICategoryService categoryService):ControllerBase {
    [HttpGet]
    public async Task<List<ReadCategoryDto>> GetCategoriesAsync() {
        Console.WriteLine("asd");
        return await categoryService.GetCategoriesAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadCategoryDto?>> GetProductById(int id) {
        return (await categoryService.GetCategoryByIdAsync(id));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto) {
        Console.WriteLine("Create Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);


        try
        {
            ReadCategoryDto category= await categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetProductById), new { id = category.Id },
            category);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, ex + "Internal server error");
        }
        
    }
}