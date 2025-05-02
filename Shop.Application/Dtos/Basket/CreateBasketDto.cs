namespace Shop.Application.Dtos.Basket;

using Abstracts;

public class CreateBasketDto:CreateDto {
    public string? UserId { get; set; }
    public  required string UserIp { get; set; }
}