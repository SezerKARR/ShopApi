namespace ShopApi.Dtos.Product;

using Models;
using ProductFilterValue;

public class CreateProductDto {

  
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public required int BrandId { get; set; } 
    public List<CreateProductValueDto>? ProductFilterValues { get; set; }= new List<CreateProductValueDto>();
    // public IFormFile? ImageFile { get; set; }
    public required decimal Price { get; set; }
    public required int CreatedByUserId { get; set; }
    public required int Quantity { get; set; }
}