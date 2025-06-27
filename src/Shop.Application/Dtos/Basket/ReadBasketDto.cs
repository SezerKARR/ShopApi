namespace Shop.Application.Dtos.Basket;

using System.Globalization;
using Abstracts;
using BasketItem;
using Domain.Models;
using User;

public class ReadBasketDto:ReadDto {
    public int Id { get; set; }
    public int UserId { get; set; }
    public ReadUserDto? User { get; set; }

    public List<ReadGroupedBasketItemsDto>? SellerGroups { get; set; }
    public decimal? GrandTotal { get; set; }
    public int TotalProductAmount {get; set;}
    public decimal? ShippingFee { get; set; }

}