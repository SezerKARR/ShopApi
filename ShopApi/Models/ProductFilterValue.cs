namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class ProductFilterValue:BaseEntity
{
    public int ProductId { get; set; }
    public required Product Product { get; set; }

    public int FilterId { get; set; }
    public required Filter Filter { get; set; }
    [MaxLength(100)]
    public string? Value { get; set; }

}
