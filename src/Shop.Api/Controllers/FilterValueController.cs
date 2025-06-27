namespace Shop.Api.Controllers;

using Application.Dtos.FilterValue;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class FilterValueController:ControllerBase {
    private readonly IFilterValueService _filterValueService;
    public FilterValueController(IFilterValueService filterValueService) {
        _filterValueService = filterValueService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllFilterValues() {

        var response = await _filterValueService.GetFilterValuesAsync();
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFilterValueById(int id) {
        var response = await _filterValueService.GetFilterValueById(id);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<IActionResult> AddFilterValue([FromBody] CreateFilterValueDto createFilterValueDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response=await _filterValueService.CreateFilterValueAsync(createFilterValueDto);
            if (response.Success)
            {
                var filterValue = response.Resource;
                if (filterValue != null)
                    return CreatedAtAction(nameof(GetFilterValueById), new
                    {
                        id = filterValue.Id
                    }, filterValue);
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