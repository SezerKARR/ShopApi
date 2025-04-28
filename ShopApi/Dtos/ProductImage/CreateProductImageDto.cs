namespace ShopApi.Dtos.ProductImage;

public class CreateProductImageDto {
    public int ProductId { get; set; }
    public int Order { get; set; } 
    public bool IsActive { get; set; }

}
