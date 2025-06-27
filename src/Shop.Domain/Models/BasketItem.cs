namespace Shop.Domain.Models;

public class BasketItem:BaseEntity {
    public int BasketId { get; set; }
    public virtual Basket? Basket { get; set; }
    public int Quantity { get; set; }
    
    
    public int ProductSellerId { get; set; }
    public virtual ProductSeller? ProductSeller { get; set; }
   
}