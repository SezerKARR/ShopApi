namespace Shop.Domain.Models;

using System.ComponentModel.DataAnnotations;

public enum Role {
    None = 0,
    User=1,

    Admin=2,
    Seller=3,
    BrandAdmin=4
}
public class User :BaseEntity{
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    public virtual Role Role { get; set; } = Role.User;
    public List<Comment>? Comments { get; set; }
    public int RoleInt
    {
        get => (int)Role;  
        set => Role = (Role)value;  
    }

    public IEnumerable<Address>? Addresses { get; }


}