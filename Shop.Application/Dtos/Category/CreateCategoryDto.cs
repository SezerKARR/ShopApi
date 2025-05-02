namespace Shop.Application.Dtos.Category;


public class CreateCategoryDto {
    public int Id { get; set; }
    public int ParentId { get; set; }
    public string? Name { get; set; }

}