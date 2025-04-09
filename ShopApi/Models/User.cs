namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;

public enum Role {
    Admin,
    User
}
public class User {
    public int Id { get; set; }
    [MaxLength(100)]
    public required string Email { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    public Role? Role { get; set; }
    public List<Product>? Products { get; set; }
}