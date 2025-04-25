namespace ShopApi.Dtos.Seller;

using User;

public class ReadSellerDto:ReadUserDto {
    public List<int> ProductSellerIds { get; set; } = new List<int>(); // Products yerine ID'ler
    public List<int> ManagedBrandIds { get; set; } = new List<int>(); // ManagedBrands yerine ID'ler
    public List<int> CreatedProductIds { get; set; } = new List<int>(); // CreatedProducts yerine ID'ler
    
}
