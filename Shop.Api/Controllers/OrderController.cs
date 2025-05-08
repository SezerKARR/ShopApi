namespace Shop.Api.Controllers;

using Application.Dtos.Order;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class OrderController:ControllerBase {
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService) {
        _orderService = orderService;
    }
    [HttpPost]
    public async Task<ActionResult<ReadOrderDto>> CreateOrder([FromBody] CreateOrderDto dto) {
        var response = await _orderService.CreateOrderAsync(dto);
        if(!response.Success)
            return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
        return Ok(response.Resource);
    }
}
