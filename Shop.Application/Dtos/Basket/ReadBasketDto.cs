namespace Shop.Application.Dtos.Basket;

using Abstracts;
using Domain.Models;

public class ReadBasketDto:ReadDto {
    public int Id { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}