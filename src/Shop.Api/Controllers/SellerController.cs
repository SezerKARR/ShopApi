namespace Shop.Api.Controllers;

using Application.Dtos.Seller;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SellerController: ControllerBase {
    private readonly ISellerService _sellerService;
    public SellerController(ISellerService sellerService) {
        _sellerService = sellerService;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadSellerDto>> GetSellerById(int id, int includes) {
        var response= await _sellerService.GetSellerByIdAsync(id, includes);
        if(response.Success)
            return Ok(response.Resource);
        return BadRequest(response.Message);
    }
}
