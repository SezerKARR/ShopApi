namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class ProductFilterValue:BaseEntity
{
    public int ProductId { get; set; }
    public int CategoryFilterId { get; set; }
    [MaxLength(100)]
    public string? Value { get; set; }
}
