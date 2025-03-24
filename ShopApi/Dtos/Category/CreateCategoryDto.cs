namespace ShopApi.Dtos.Category;

using Models;

public class CreateCategoryDto {
    public int Id { get; set; }
    public int ParentId { get; set; }
    public string? Name { get; set; }

}