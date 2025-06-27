namespace Shop.Application.Dtos.Image;

using Microsoft.AspNetCore.Http;

public class CreateImageDto {
    public required IFormFile ImageFile { get; set; }
    public string? AltText { get; set; } 
}
