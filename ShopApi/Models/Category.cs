namespace ShopApi.Models;

using Abstracts;

public class Category:BaseEntity {

    public List<Product> Products { get; set; } = new List<Product>();
}