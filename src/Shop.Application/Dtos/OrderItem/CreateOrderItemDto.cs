namespace Shop.Application.Dtos.OrderItem;

public class CreateOrderItemDto {
    public int ProductSellerId { get; set; } 
    public int Quantity { get; set; }
}
