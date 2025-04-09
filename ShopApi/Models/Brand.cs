namespace ShopApi.Models;

using Abstracts;

public class Brand : BaseEntity 
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } 

    public string? LogoUrl { get; set; } 

    public string? Slug { get; set; } 

    public ICollection<Product> Products { get; set; } = new List<Product>(); 
}