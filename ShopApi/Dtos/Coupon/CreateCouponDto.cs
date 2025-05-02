namespace ShopApi.Dtos.Coupon;

public class CreateCouponDto {
    public int SellerId { get; set; }
    public int? Reduction { get; set; }
    public int? MinLimit { get; set; }
    public int? MaxUsageCount { get; set; }
    public DateTime ValidUntil { get; set; }
    
}
