namespace Shop.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class ProductFilterValue:BaseEntity
{
    public int ProductId { get; set; }
    public  Product? Product { get; set; }
    
    public int FilterId { get; set; }
    public Filter? Filter { get; set; }
    public int FilterValueId { get; set; } 
    public FilterValue? FilterValue { get; set; }
    [MaxLength(100)]
    public string? CustomValue { get; set; }

}
