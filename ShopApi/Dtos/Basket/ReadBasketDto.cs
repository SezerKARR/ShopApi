namespace ShopApi.Dtos.Basket;

using Abstracts;
using Models;

public class ReadBasketDto:ReadDto {
    public int Id { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}