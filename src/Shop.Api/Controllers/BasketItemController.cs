namespace Shop.Api.Controllers;

using Application.Dtos.BasketItem;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class BasketItemController:ControllerBase {
    private readonly IBasketItemService _basketItemService;
    public BasketItemController(IBasketItemService basketItemService) {
        _basketItemService = basketItemService;
    }
   
    [HttpPost]
    public async Task<ActionResult<ReadBasketItemDto>> CreateBasketItem(CreateBasketItemDto createBasketItemDto) {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await _basketItemService.CreateBasketItemAsync(createBasketItemDto,1);
            if (!response.Success) return BadRequest(response.Message);
            var basketItemDto = response.Resource;
            return Ok(basketItemDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
