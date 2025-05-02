namespace Shop.Api.Controllers;

using Application.Dtos.Basket;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BasketController :ControllerBase {
    readonly IBasketService _basketService;
    public BasketController(IBasketService basketService) {
        _basketService = basketService;
    }
    [HttpGet("GetAll")]
    public async Task<ActionResult<ReadBasketDto>> GetBaskets() {
        var response = await _basketService.GetBasketsAsync();
        if (response.Success)
        {
            var basketDto = response.Resource;
            return Ok(basketDto);
        }
        return BadRequest(response.Message);
    }
    [HttpGet("Basket")]
    public async Task<ActionResult<ReadBasketDto>> GetBasket([FromBody] CreateBasketDto createBasketDto) {
        
        var response = await _basketService.GetBasketAsync(createBasketDto);
        if (response.Success)
        {
            var basketDto = response.Resource;
            return Ok(basketDto);
        }
        else
        {
            return await CreateBasket(createBasketDto);
        }
        
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadBasketDto>> GetBasketById([FromRoute] int id) {
        var response = await _basketService.GetBasketByIdAsync(id);
        if (response.Success)
        {
            var basketDto = response.Resource;
            return Ok(basketDto);
        }
        return BadRequest(response.Message);
    }
    [HttpPost]
    public async Task<ActionResult<ReadBasketDto>> CreateBasket([FromBody]CreateBasketDto basket) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await _basketService.CreateBasketAsync(basket);
            if (response.Resource!=null)
            {
                var basketDto = response.Resource;
                return CreatedAtAction(nameof(GetBasketById),new{id=basketDto.Id}
                ,basketDto);
            }
            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex + "Internal server error");
        }
    }


}