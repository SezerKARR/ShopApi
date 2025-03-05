namespace ShopApi.Controllers;

using AutoMapper;
using Dtos.Category;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("api/[controller]")]
[ApiController] 
public class CategoryController(ICategoryService categoryService,IMapper mapper):ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetCategories()
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id) {
        var response = await categoryService.GetCategoryByIdAsync(id);
        if (response.Success)
        {
            ReadCategoryDto category =mapper.Map<ReadCategoryDto>(response.Resource);
            return Ok(category);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto) {
        Console.WriteLine("Create Product");
        if (!ModelState.IsValid) return BadRequest(ModelState);


        try
        {
            var response = await categoryService.CreateCategoryAsync(createCategoryDto);
            if (response.Success)
            {
                var category = mapper.Map<ReadCategoryDto>(response.Resource);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);

            }
            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, ex + "Internal server error");
        }
        
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryDto updateCategoryDto) {
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id) {
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