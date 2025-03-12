namespace ShopApi.Models;

using Abstracts;

public class AppUser:BaseEntity {
    public AppUser(int id, string? name, string slug) : base(id, name, slug) {
    }
}