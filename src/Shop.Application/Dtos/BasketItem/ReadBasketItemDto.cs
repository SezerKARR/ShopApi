namespace Shop.Application.Dtos.BasketItem;

using System.Globalization;
using Abstracts;
using Domain.Models;
using Product;
using ProductImage;

public class ReadBasketItemDto:ReadDto{
    public int Id { get; set; }
    public int Quantity { get; set; }
    // public string BrandName { get; set; }
    public ReadProductDto? Product { get; set; }
    public decimal Price { get; set; }
    public int ProductSellerId { get; set; }
   
    public decimal TotalPrice=>Quantity*Price;
}