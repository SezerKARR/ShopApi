namespace ShopApi.Dtos.Category;

using Models;

public class ReadCategoryDto {
    
    public int Id { get; set; }
    public string? Slug { get; set; }

    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public List<ReadCategoryDto> SubCategories { get; set; } = new List<ReadCategoryDto>();
    public List<Product> Products { get; set; } = new List<Product>();
}