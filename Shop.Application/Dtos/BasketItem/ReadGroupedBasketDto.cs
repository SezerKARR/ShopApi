namespace Shop.Application.Dtos.BasketItem;

public class ReadGroupedBasketItemsDto {
    public int SellerId { get; set; }
    public string SellerName { get; set; }
    public virtual List<ReadBasketItemDto> Items { get; set; }
    public decimal Subtotal { get; set; }
}
