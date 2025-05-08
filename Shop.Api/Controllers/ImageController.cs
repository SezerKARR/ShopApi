namespace Shop.Api.Controllers;

using Application.Dtos.Image;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class ImageController:ControllerBase {
    private readonly IImageService _imageService;
    public ImageController(IImageService imageService) {
        _imageService = imageService;
    }
    [HttpPost]
    public async Task<ActionResult<ReadImageDto>> CreateProductSeller(CreateImageDto createImageDto) {
        var response = await _imageService.CreateImageAsync(createImageDto);
        if (response.Success)
        {
            return Ok(response.Resource);
        }
        return BadRequest(response.Message);
    }
}
