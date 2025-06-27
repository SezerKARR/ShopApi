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
    [HttpPut("updateItemsForQuantity")]
    public async Task<ActionResult<ReadBasketItemDto>> UpdateBasketItem([FromBody] List<UpdateBasketItemDto> updateBasketItemDtos) {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid model");
        }
        try
        {
            var response= await _basketItemService.UpdateBasketItemsQuantityAsync(updateBasketItemDtos);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }            
            return Ok(response.Resource);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}
