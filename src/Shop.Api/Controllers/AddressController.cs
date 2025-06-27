namespace Shop.Api.Controllers;

using Application.Dtos.Address;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class AddressController: ControllerBase{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService) {
        _addressService = addressService;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadAddressDto>> GetAddressById(int id) {
        var response = await _addressService.GetAddressByIdAsync(id);
        if(!response.Success)return BadRequest(response.Message);
        Console.WriteLine("sa");
        return Ok(response.Resource);
    }
    [HttpPost]
    public async Task<ActionResult<ReadAddressDto>> CreateAddress(CreateAddressDto dto) {
        var response=await _addressService.CreateAddress(dto);
        if(!response.Success)return BadRequest(response.Message);
        return Ok(response.Resource);
    }

}
