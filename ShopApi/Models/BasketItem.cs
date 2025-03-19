namespace ShopApi.Models;

using Abstracts;

public class BasketItem:BaseEntity {
    public int BasketId { get; set; }
    public int Quantity { get; set; }
    public required Product Product { get; set; }
    public decimal TotalPrice => Product.Price * Quantity;
}