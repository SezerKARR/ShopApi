namespace Shop.Application.Dtos.Product;

public class UpdateProductDto {
    public int? SellerId { get; set; }
    public bool IsShippingFree { get; set; } = false;
}