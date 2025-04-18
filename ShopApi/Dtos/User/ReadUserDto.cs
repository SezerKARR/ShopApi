namespace ShopApi.Dtos.User;

using Brand;
using Models;
using Product;

public class ReadUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<int> ProductSellerIds { get; set; } = new List<int>(); // Products yerine ID'ler
    public List<int> ManagedBrandIds { get; set; } = new List<int>(); // ManagedBrands yerine ID'ler
    public List<int> CreatedProductIds { get; set; } = new List<int>(); // CreatedProducts yerine ID'ler
}