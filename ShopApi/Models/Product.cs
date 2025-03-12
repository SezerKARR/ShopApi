namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Product: BaseEntity{

    public Product(int id, string? name, string slug, string description, int categoryId, string imageUrl) : base(id, name, slug) {
        Description = description;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
    }
    [MaxLength(180)]
    public string Description { get; set; }
    public int CategoryId { get; set; }
    [MaxLength(120)]
    public string ImageUrl { get; set; }
    
}