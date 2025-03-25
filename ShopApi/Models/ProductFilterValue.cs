namespace ShopApi.Models;

using Abstracts;

public class ProductFilterValue:BaseEntity
{
    public int ProductId { get; set; }
    public int CategoryFilterId { get; set; }
    public string? Value { get; set; }
}
