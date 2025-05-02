namespace ShopApi.Dtos.Coupon;

using System.Runtime.InteropServices.JavaScript;
using ProductSeller;
using Seller;

public class ReadCouponDto {
    public int Id { get; set; }
    public int SellerId { get; set; }
    public ReadSellerDto? Seller { get; set; }
    public int? Reduction { get; set; }
    public int? MinLimit { get; set; }
    public int? MaxUsageCount { get; set; }
    public int UsageCount { get; set; }
    public DateTime ValidUntil  { get; set; }
    public bool IsExpired => DateTime.UtcNow > ValidUntil;
    public DateOnly OnlyDate => DateOnly.FromDateTime(ValidUntil);

    public string Time => ValidUntil.TimeOfDay.ToString(@"hh\:mm\:ss");


}
