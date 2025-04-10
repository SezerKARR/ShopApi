namespace ShopApi.Dtos.Product;

using Models;
using ProductFilterValue;

public class CreateProductDto {

  
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CategoryId { get; set; }
    public required string Brand { get; set; } 
    public List<int> ProductFilterValues { get; set; }= new List<int>();
    public IFormFile? ImageFile { get; set; }
    public decimal Price { get; set; }
    public required int CreatedByUserId { get; set; }
}