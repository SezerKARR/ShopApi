namespace ShopApi.Models;

public class ProductFilterValue
{
    public int ProductId { get; set; }
    public int CategoryFilterId { get; set; }
    public string? Value { get; set; }
}
