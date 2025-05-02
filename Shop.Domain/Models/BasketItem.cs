namespace Shop.Domain.Models;

public class BasketItem:BaseEntity {
    public int BasketId { get; set; }
    public required Basket Basket { get; set; }
    public int Quantity { get; set; }
    
    public int ProductSellerId { get; set; }
    public required ProductSeller ProductSeller { get; set; }
    public decimal TotalPrice => ProductSeller.Price * Quantity;
}