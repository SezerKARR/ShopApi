namespace ShopApi.Models;

using Abstracts;

public class Stock:BaseEntity {
    public int ProductSellerId { get; set; }
    public  ProductSeller? ProductSeller { get; set; }
    public DateTime DateTime { get; set; }
    public int Quantity { get; set; }
    
    
}