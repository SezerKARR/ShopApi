namespace Shop.Application.Dtos.ProductImage;

public class ReadProductImageDto {
    public int Id { get; set; }
    public string? Url { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
