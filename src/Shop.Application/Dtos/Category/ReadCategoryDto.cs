namespace Shop.Application.Dtos.Category;

public class ReadCategoryDto
{
    public int Id { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public List<int> SubCategoryIds { get; set; } = new List<int>(); // SubCategories yerine ID'ler
    public List<int> CategoryFilterIds { get; set; } = new List<int>();
    public List<int> ProductIds { get; set; } = new List<int>();   // Products yerine ID'ler
    // public List<Filter> Filters { get; set; } = new List<Filter>();
}