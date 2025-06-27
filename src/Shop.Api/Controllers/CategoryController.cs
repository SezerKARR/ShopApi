namespace Shop.Api.Controllers;

using Application.Dtos.Category;
using Microsoft.AspNetCore.Mvc;
using Application.Services;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<ReadCategoryDto>> GetCategories() {
        var response = await categoryService.GetCategoriesAsync();
        if (response.Success)
        {
            var categories = response.Resource;

            return Ok(categories);
        }

        return BadRequest(response.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadCategoryDto>> GetCategoryById([FromRoute] int id) {
        Console.WriteLine("asdsad");
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
            if (response.Resource != null)
            {
                var category = response.Resource;
                return CreatedAtAction(nameof(GetCategoryById), new
                {
                    id = category.Id
                }, category);

            }
            return BadRequest(response.Message);
        }
        catch (Exception ex) { return StatusCode(500, ex + "Internal server error"); }

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
    // [HttpGet("main/{id}/subcategories")]
    // public async Task<ActionResult<IEnumerable<ReadCategoryDto>>> GetSubcategoriesByMainCategory(int id) {
    //     var response = await categoryService.GetSubcategoriesByMainCategory(id);
    //     if (response.Success)
    //     {
    //         var category = response.Resource;
    //         return Ok(category);
    //     }
    //     return BadRequest(response.Message);
    //
    //    
    // }
    //
    // // Belirli bir ana kategorideki ürünleri getir (tüm alt kategorilerdeki ürünler dahil)
    // [HttpGet("main/{id}/products")]
    // public async Task<ActionResult<IEnumerable<ReadCategoryDto>>> GetProductsByMainCategory(int id) {
    //     // Önce bu ana kategorinin tüm alt kategori ID'lerini bulalım
    //     var categoryIds = new List<int>
    //     {
    //         id
    //     };
    //
    //     // Alt ana kategorileri bul
    //     var subMainIds = await _context.Categories
    //         .Where(c => c.ParentId == id && c.Type == CategoryType.SubMainCategory)
    //         .Select(c => c.Id)
    //         .ToListAsync();
    //
    //     categoryIds.AddRange(subMainIds);
    //
    //     // Normal kategorileri bul
    //     var normalIds = await _context.Categories
    //         .Where(c => subMainIds.Contains(c.ParentId.Value) && c.Type == CategoryType.NormalCategory)
    //         .Select(c => c.Id)
    //         .ToListAsync();
    //
    //     categoryIds.AddRange(normalIds);
    //
    //     // Bu kategorilerdeki tüm ürünleri getir
    //     return await _context.Products
    //         .Where(p => categoryIds.Contains(p.CategoryId))
    //         .ToListAsync();
    // }
}