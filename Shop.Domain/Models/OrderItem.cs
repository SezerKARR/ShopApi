namespace Shop.Domain.Models;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;


    public int ProductSellerId { get; set; } 
    public virtual ProductSeller ProductSeller { get; set; } = null!; 

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
