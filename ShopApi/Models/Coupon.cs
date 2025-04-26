namespace ShopApi.Models;

using Abstracts;
using Expired;


public class Coupon:BaseEntity,IExpirable {
    public int SellerId { get; set; }
    public Seller? Seller { get; set; }
    public int? Reduction { get; set; }
    public int? MinLimit { get; set; }
    public int? MaxUsageCount { get; set; }
    public int UsageCount { get; set; }

    public DateTime ValidUntil { get; set; }
    public bool IsExpired => DateTime.UtcNow > ValidUntil;
}
