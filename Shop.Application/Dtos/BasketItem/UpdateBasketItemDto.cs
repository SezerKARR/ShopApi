namespace Shop.Application.Dtos.BasketItem;

using Abstracts;
using Domain.Models;

public class UpdateBasketItemDto:UpdateDto {
    public int Id { get; set; }
    public int BasketId { get; set; }
    public int Quantity { get; set; }
    public required Product Product { get; set; }
}