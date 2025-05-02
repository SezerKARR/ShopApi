namespace Shop.Application.Dtos.Product;

using ProductFilterValue;
using ProductImage;

public class CreateProductDto {

  
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public required int BrandId { get; set; } 
    public List<CreateProductValueDto>? ProductFilterValues { get; set; }= new List<CreateProductValueDto>();
    public required List<CreateProductImageDto>? ProductImages { get; set; }= new List<CreateProductImageDto>();
    public required decimal Price { get; set; }
    public required int CreatedByUserId { get; set; }
    public required int Quantity { get; set; }
}