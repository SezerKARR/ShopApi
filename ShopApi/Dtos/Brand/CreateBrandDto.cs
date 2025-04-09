namespace ShopApi.Dtos.Brand;

using System.ComponentModel.DataAnnotations;

public class CreateBrandDto {
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }
}