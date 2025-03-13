namespace ShopApi.Controllers;

using Dtos.MainCategory;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("api/[controller]")]
[ApiController]
public class MainCategoryController(IMainCategoryService mainCategoryService):ControllerBase{
    [HttpGet]
    public async Task<ActionResult<ReadMainCategory>> GetMainCategories() {
        var response = await mainCategoryService.GetMainCategoriesAsync();
        if (response.Success)
        {
            List<ReadMainCategory>? readMainCategoryCategories = response.Resource;
            return Ok(readMainCategoryCategories);
        }
        return BadRequest(response.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadMainCategory>> GetMainCategoryById([FromRoute] int id) {
        var response = await mainCategoryService.GetMainCategoryById(id);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<ActionResult<ReadMainCategory>> CreateMainCategory([FromBody] CreateMainCategory createMainCategory) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mainCategoryService.CreateMainCategory(createMainCategory);
            if (response.Resource!=null)
            {
                var readMainCategory = response.Resource;
                return CreatedAtAction(nameof(GetMainCategoryById),new {id=readMainCategory.Id}, readMainCategory);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e + "Internal server error");
        }
    }
}