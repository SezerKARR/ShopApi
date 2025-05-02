namespace ShopApi.Controllers;

using Dtos.Brand;
using Dtos.BrandCategory;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("api/[controller]")]
[ApiController]
public class BrandCategoryController:ControllerBase {
    private readonly IBrandCategoryService _brandCategoryService;

    public BrandCategoryController(IBrandCategoryService brandCategoryService) {
        _brandCategoryService = brandCategoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrandCategories() {
        var response = await _brandCategoryService.GetAllAsync();
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandCategoryById(int id) {
        var response = await _brandCategoryService.GetByIdAsync(id);
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);
    }
    [HttpGet("brandsByCategoryId/{categoryId}")]
    public async Task<ActionResult<List<ReadBrandDto>>> GetBrandsByCategoryId(int categoryId) {
        var response = await _brandCategoryService.GetBrandsByCategoryIdAsync(categoryId);
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<IActionResult> AddBrandCategory([FromBody] CreateBrandCategoryDto createBrandCategoryDto) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _brandCategoryService.CreateAsync(createBrandCategoryDto);
            if (response.Success)
            {
                var created = response.Resource;
                if (created != null)
                    return CreatedAtAction(nameof(GetBrandCategoryById), new
                    {
                        id = created.Id
                    }, created);
            }
            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }
}