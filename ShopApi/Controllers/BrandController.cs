namespace ShopApi.Controllers;

using Dtos.Brand;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("api/[controller]")]
[ApiController]
public class BrandController:ControllerBase {
    readonly IBrandService _brandService;

    public BrandController(IBrandService brandService) {
        _brandService = brandService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ReadBrandDto>>> GetBrands(int includes = -1) {
        var response = await _brandService.GetBrandsAsync(includes);
        if(response.Success) return Ok(response.Resource);
        return BadRequest(response.Message);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<ReadBrandDto>>> GetBrandsById(int id,int includes=-1) {
        Console.WriteLine(id);

        var response = await _brandService.GetBrandByIdAsync(id,includes);
        if(response.Success) return Ok(response.Resource);
        return BadRequest(response.Message);
    }
}