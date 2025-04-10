namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;

public enum Role {
    None = 0,
    Admin=1,
    User=2,
    Seller=3,
    BrandAdmin=4
}
public class User {
    public int Id { get; set; }
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    public Role Role { get; set; }
    public List<Product>? Products { get; set; }
    public ICollection<Brand> ManagedBrands { get; set; } = new List<Brand>();
}