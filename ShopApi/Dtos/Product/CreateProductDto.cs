namespace ShopApi.Dtos.Product;

public class CreateProductDto {

  
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public IFormFile? ImageFile { get; set; }
    public decimal Price { get; set; }
    public int? SellerId { get; set; }
}