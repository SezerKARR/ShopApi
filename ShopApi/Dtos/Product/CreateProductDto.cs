namespace ShopApi.Dtos.Product;

public class CreateProductDto {

  
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CategoryId { get; set; }
    public required string Brand { get; set; } 
    public IFormFile? ImageFile { get; set; }
    public decimal Price { get; set; }
    public required int SellerId { get; set; }
}