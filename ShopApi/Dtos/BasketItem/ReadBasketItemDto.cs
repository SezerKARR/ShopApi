namespace ShopApi.Dtos.BasketItem;

using Abstracts;
using Models;

public class ReadBasketItemDto:ReadDto{
    public int Id { get; set; }
    public int BasketId { get; set; }
    public int Quantity { get; set; }
    public required Product Product { get; set; }
}