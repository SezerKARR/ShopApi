namespace Shop.Application.Dtos.Basket;

using Abstracts;

public class UpdateBasketDto:UpdateDto{
    public decimal ShippingFee { get; set; }

}