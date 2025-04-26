namespace ShopApi.Models;

public class Seller:User {
    
    public override Role Role { get; set; } = Role.Seller;
    public ICollection<ProductSeller>? ProductSellers { get; set; } = new List<ProductSeller>();
    public ICollection<Brand>? ManagedBrands { get; set; } = new List<Brand>();
    public IEnumerable<Product>? CreatedProducts { get; set; }
    public IEnumerable<Coupon>? Coupons { get; set; }
}
