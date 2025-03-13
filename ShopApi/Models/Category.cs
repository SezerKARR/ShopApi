namespace ShopApi.Models;

using Abstracts;

public class Category:BaseEntity {
    public int? mainCategoryId;

    public List<Product> Products { get; set; } = new List<Product>();
}