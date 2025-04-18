namespace ShopApi.Dtos.BrandCategory;

using Models;

public class ReadBrandCategoryDto {
    public int Id { get; set; }
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}