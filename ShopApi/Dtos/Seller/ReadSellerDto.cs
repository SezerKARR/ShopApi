namespace ShopApi.Dtos.Seller;

using Coupon;
using User;

public class ReadSellerDto:ReadUserDto {
    public List<int> ProductSellerIds { get; set; } = new List<int>(); // Products yerine ID'ler
    public List<int> ManagedBrandIds { get; set; } = new List<int>(); // ManagedBrands yerine ID'ler
    public List<int> CreatedProductIds { get; set; } = new List<int>(); // CreatedProducts yerine ID'ler
    public List<ReadCouponDto> Coupons { get; set; } = new List<ReadCouponDto>();
    
}
