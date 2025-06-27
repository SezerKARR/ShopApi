namespace Shop.Application.Dtos.BrandCategory;

using Domain.Models;

public class ReadBrandCategoryDto {
    public int Id { get; set; }
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}