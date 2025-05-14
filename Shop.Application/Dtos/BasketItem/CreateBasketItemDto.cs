namespace Shop.Application.Dtos.BasketItem;

using Abstracts;
using Domain.Models;

public class CreateBasketItemDto:CreateDto {
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public int ProductSellerId { get; set; }
}