namespace ShopApi.Dtos.Brand;

public class ReadBrandDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public string? Slug { get; set; }
}
