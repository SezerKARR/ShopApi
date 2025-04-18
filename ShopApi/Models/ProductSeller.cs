namespace ShopApi.Models;

using Abstracts;

public class ProductSeller:BaseEntity
{
    public  int ProductId { get; set; }
    public  Product? Product { get; set; }

    public  int SellerId { get; set; }
    public  User? Seller { get; set; }
    public List<Stock>? Stocks { get; set; } = new List<Stock>();

}