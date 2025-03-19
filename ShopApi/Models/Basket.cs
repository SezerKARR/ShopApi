namespace ShopApi.Models;

using Abstracts;

public class Basket : BaseEntity {
    public string? UserId { get; set; }
    public  required string UserIp { get; set; }
    public List<Product>? Products { get; set; }
}