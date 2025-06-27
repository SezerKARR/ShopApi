namespace Shop.Application.Dtos.BasketItem;

using System.Globalization;

public class ReadGroupedBasketItemsDto {
    public int SellerId { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public List<ReadBasketItemDto> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal FreeShippingMinimumOrderAmount { get; set; }
    public bool IsShippingFree { get; set; }
}
