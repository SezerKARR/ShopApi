namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public enum Role {
    None = 0,
    Admin=1,
    User=2,
    Seller=3,
    BrandAdmin=4
}
public class User :BaseEntity{
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    public Role Role { get; set; }
    public ICollection<ProductSeller>? ProductSellers { get; set; } = new List<ProductSeller>();
    public ICollection<Brand>? ManagedBrands { get; set; } = new List<Brand>();
    public IEnumerable<Product>? CreatedProducts { get; set; }
}