namespace ShopApi.Controllers;

using Dtos.Category;
using Interface;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController] 
public class CategoryController(ICategoryService categoryService):ControllerBase {
    [HttpGet]
    public async Task<ActionResult<ReadCategoryDto>> GetCategories()
    {
        var response = await categoryService.GetCategoriesAsync();
        
        if (response.Success)
        {
            var categories = response.Resource; 
            Console.WriteLine(categories);
            return Ok(categories);
        }
        
        return BadRequest(response.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadCategoryDto>> GetCategoryById([FromRoute] int id) {
        var response = await categoryService.GetCategoryByIdAsync(id);
        if (response.Success)
        {
            var category = response.Resource;
            return Ok(category);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<ActionResult<ReadCategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);


        try
        {
            var response = await categoryService.CreateCategoryAsync(createCategoryDto);
            if (response.Resource!=null)
            {
                var category = response.Resource;
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);

            }
            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex + "Internal server error");
        }
        
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ReadCategoryDto>> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryDto updateCategoryDto) {
        Console.WriteLine("Update Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
       var response = await categoryService.UpdateCategoryAsync(id, updateCategoryDto);
       if (response.Success)
       {
           var category = response.Resource;
           return Ok(category);
       }
       return BadRequest(response.Message);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ReadCategoryDto>> DeleteCategory([FromRoute] int id) {
        Console.WriteLine("Delete Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await categoryService.DeleteCategoryAsync(id);
        if (response.Success)
        {
            var category = response.Resource;
            return Ok(category);
        }
        return BadRequest(response.Message);
    }
}