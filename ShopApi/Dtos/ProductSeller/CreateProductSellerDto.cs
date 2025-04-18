namespace ShopApi.Dtos.ProductSeller;

public class CreateProductSellerDto {
    public required int ProductId { get; set; }
    public required int SellerId { get; set; }
    public required int Quantity { get; set; }

}