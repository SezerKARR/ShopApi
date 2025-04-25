namespace ShopApi.Models;

using Abstracts;

public class ProductSeller:BaseEntity
{
    public  int ProductId { get; set; }
    public  Product? Product { get; set; }
    
    public decimal Price { get; set; }
    public  int SellerId { get; set; }
    public  Seller? Seller { get; set; }
    public List<Stock>? Stocks { get; set; } = new List<Stock>();

}