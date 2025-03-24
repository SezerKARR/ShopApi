namespace ShopApi.Dtos.Basket;

using Abstracts;
using Models;

public class CreateBasketDto:CreateDto {
    public string? UserId { get; set; }
    public  required string UserIp { get; set; }
}