namespace Shop.Application.Dtos.Seller;

using Coupon;
using User;

public class ReadSellerDto:ReadUserDto {
    public List<int> ProductSellerIds { get; set; } = new List<int>(); 
    public List<int> ManagedBrandIds { get; set; } = new List<int>(); 
    public List<int> CreatedProductIds { get; set; } = new List<int>(); 
    public List<ReadCouponDto> Coupons { get; set; } = new List<ReadCouponDto>();
    
}
