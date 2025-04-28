namespace ShopApi.Models;

using Abstracts;

public class ProductImage:BaseEntity {
    public int ProductId { get; set; }
    public string Url { get; set; }
    public int Order { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product Product { get; set; }
}
