namespace ShopApi.Models;

using Abstracts;
using Interface;

public class Product: BaseEntity{
    
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
}