namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Product: BaseEntity{

  
    [MaxLength(180)]
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    [MaxLength(120)]
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public List<ProductFilterValue>? FilterValues { get; set; }
    
}