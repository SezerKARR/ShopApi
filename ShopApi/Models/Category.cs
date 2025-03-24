namespace ShopApi.Models;

using Abstracts;


public class Category:BaseEntity {
    public int? ParentId { get; set; }
    public Category? ParentCategory { get; set; } // Bu kategoriye ait ana kategori
    public List<Category> SubCategories { get; set; } = new List<Category>(); // Alt kategoriler

    public List<Product> Products { get; set; } = new List<Product>();
}

// public enum CategoryType {
//     None = 0,
//     Main = 1,
//     SubMain = 2,
//     SubSubMain = 3,
//     
// }