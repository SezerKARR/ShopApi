namespace ShopApi.Dtos.ProductSeller;

using Coupon;
using Models;
using Product;
using Seller;
using Stock;
using User;

public class ReadProductSellerDto
{
    public int Id { get; set; }
    public required int ProductId { get; set; }
    public required int SellerId { get; set; }
    public decimal Price { get; set; }

    public ReadProductDto? Product { get; set; } 
    public ReadSellerDto? Seller { get; set; }
    public List<int> StockIds { get; set; } = new List<int>();
}