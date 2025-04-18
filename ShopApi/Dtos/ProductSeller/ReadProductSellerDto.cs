namespace ShopApi.Dtos.ProductSeller;

using Product;
using Stock;
using User;

public class ReadProductSellerDto
{
    public required int ProductId { get; set; }
    public required int SellerId { get; set; }
    public List<int> StockIds { get; set; } = new List<int>();
}