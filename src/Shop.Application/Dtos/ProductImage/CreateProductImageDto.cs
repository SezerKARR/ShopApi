namespace Shop.Application.Dtos.ProductImage;

using Microsoft.AspNetCore.Http;

public class CreateProductImageDto {
    public int ProductId { get; set; }
    public int Order { get; set; } 
    public bool IsActive { get; set; }
    public int ImageId { get; set; }
}
