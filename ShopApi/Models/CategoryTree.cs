namespace ShopApi.Models;

public class CategoryTree
{
    public int AncestorId { get; set; }
    public int DescendantId { get; set; }
    public int Depth { get; set; }

    public Category Ancestor { get; set; } = null!;
    public Category Descendant { get; set; } = null!;
}