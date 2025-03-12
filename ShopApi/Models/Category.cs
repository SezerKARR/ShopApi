namespace ShopApi.Models;

using Abstracts;

public class Category:BaseEntity {

    public Category(int id, string? name, string slug) : base(id, name, slug) {
    }
    public List<Product> Products { get; set; } = new List<Product>();
}