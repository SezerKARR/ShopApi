namespace Shop.Application.Dtos.ProductImage;

using Image;
using Product;

public class ReadProductImageDto {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public virtual ReadProductDto? Product { get; set; }
    public int ImageId { get; set; }
    public virtual ReadImageDto? Image { get; set; }
    public int Order { get; set; } = 0; 
}
