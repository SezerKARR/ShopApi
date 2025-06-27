namespace Shop.Api.Controllers;

using Application.Dtos.ProductFilterValue;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductFilterValueController:ControllerBase {
    private readonly IProductFilterValueService _productFilterValueService;
    public ProductFilterValueController(IProductFilterValueService productFilterValueService) {
        _productFilterValueService = productFilterValueService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllFilterValues() {

        var response = await _productFilterValueService.GetProductFilterValuesAsync();
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFilterValueById(int id) {
        var response = await _productFilterValueService.GetProductFilterValueById(id);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<IActionResult> AddFilterValue([FromBody] CreateProductValueDto createProductValueDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response=await _productFilterValueService.CreateProductFilterValueAsync(createProductValueDto);
            if (response.Success)
            {
                var productFilterValueResource = response.Resource;
                if (productFilterValueResource != null)
                    return CreatedAtAction(nameof(GetFilterValueById), new
                    {
                        id = productFilterValueResource.Id
                    }, productFilterValueResource);
            }
            return BadRequest(response.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}