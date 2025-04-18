namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class ProductFilterValue:BaseEntity
{
    public int ProductId { get; set; }
    public  Product? Product { get; set; }
    
    
    
    public int FilterValueId { get; set; } 
    public FilterValue? FilterValue { get; set; }
    [MaxLength(100)]
    public string? CustomValue { get; set; }

}
