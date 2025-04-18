// namespace ShopApi.Controllers;
//
// using Dtos.Brand;
// using Microsoft.AspNetCore.Mvc;
// using Services;
//
// [Route("api/[controller]")]
// [ApiController]
// public class BrandController:ControllerBase {
//     readonly IBrandService _brandService;
//
//     BrandController(IBrandService brandService) {
//         _brandService = brandService;
//     }
//     [HttpGet]
//     public async Task<ActionResult<List<ReadBrandDto>>> GetBrands() {
//         var response = await _brandService.GetBrandsAsync();
//         if(response.Success) return Ok(response.Resource);
//         return BadRequest(response.Message);
//     }
//     [HttpGet("by-category/{categoryId}")]
//     public async Task<ActionResult<List<ReadBrandDto>>> GetBrandsByCategory(int categoryId) {
//         Console.WriteLine(categoryId);
//         
//         var response=_brandService.
//     }
// }