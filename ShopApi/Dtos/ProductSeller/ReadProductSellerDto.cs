namespace ShopApi.Dtos.ProductSeller;

using Comment;
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
    public  string? ProductName => Product?.Name;
    public required int SellerId { get; set; }
    public  string? SellerName => Seller?.Name;
    public decimal Price { get; set; }
    public List<ReadCommentDto> Comments { get; set; } = new List<ReadCommentDto>();


    public ReadProductDto? Product { get; set; } 
    public ReadSellerDto? Seller { get; set; }
    public List<int> StockIds { get; set; } = new List<int>();
}