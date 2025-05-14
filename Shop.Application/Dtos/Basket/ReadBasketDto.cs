namespace Shop.Application.Dtos.Basket;

using Abstracts;
using BasketItem;
using Domain.Models;
using User;

public class ReadBasketDto:ReadDto {
    public int Id { get; set; }
    public int UserId { get; set; }
    public ReadUserDto? User { get; set; }

    public List<ReadGroupedBasketItemsDto>? SellerGroups { get; set; }
    public decimal GrandTotal { get; set; }
}