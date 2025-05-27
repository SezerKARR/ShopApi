namespace Shop.Application.Dtos.Basket;

using Abstracts;

public class CreateBasketDto:CreateDto {
    public int? UserId { get; set; }
   public decimal ShippingFee { get; set; }
} 
    
