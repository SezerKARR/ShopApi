namespace ShopApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
[Route("api/[controller]")]
[ApiController]
public class FilterController : ControllerBase {
    readonly IFilterService _filterService;
    public FilterController(IFilterService filterService) {
        _filterService = filterService;

    }
    [HttpGet]
    public async Task<ActionResult<List<Filter>>> GetAllFilters() {
        var response = await _filterService.GetFilters();
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Filter>> GetFilterById(int id) {
        var response = await _filterService.GetFilterById(id);
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);

    }
    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<Filter>> GetFilterByCategoryId(int categoryId) {
        var response = await _filterService.GetFiltersByCategoryId(categoryId);
        if (response.Success) { return Ok(response.Resource); }
        return BadRequest(response.Message);

    }
    [HttpPost]
    public async Task<ActionResult<Filter>> CreateFilter([FromBody]Filter filter) {
        var response = await _filterService.CreateFilter(filter);
        if (response.Success)
        {
            return CreatedAtAction("GetFilterById", new
            {
                id = filter.Id
            }, response.Resource);
        }
        return BadRequest(response.Message);
    }
}