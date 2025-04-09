namespace ShopApi.Dtos.Product;

using Models;

public class ReadProductDto {

    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Slug { get; set; }
    public string? ImageUrl { get; set; }
    public required string Brand { get; set; } 
    public List<ProductFilterValue>? FilterValues { get; set; } = new List<ProductFilterValue>();
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int? SellerId { get; set; }
}