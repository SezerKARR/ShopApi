namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Basket : BaseEntity {
    [MaxLength(120)]
    public string? UserId { get; set; }
    [MaxLength(120)]
    public  required string UserIp { get; set; }

    public List<BasketItem>? BasketItems { get; set; }
}