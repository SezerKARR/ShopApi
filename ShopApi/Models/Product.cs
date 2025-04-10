namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Product: BaseEntity{

  
    [MaxLength(180)]
    public string? Description { get; set; }
    public required int CategoryId { get; set; }
    public required Category Category { get; set; }
    [MaxLength(120)]
    public string? ImageUrl { get; set; }
    [MaxLength(150)]
    public required string Brand { get; set; } 
    public decimal Price { get; set; }
    public List<ProductFilterValue>? FilterValues { get; set; } = new List<ProductFilterValue>();
    public required int CreatedByUserId { get; set; }
    public required User CreatedByUser { get; set; }
    
}