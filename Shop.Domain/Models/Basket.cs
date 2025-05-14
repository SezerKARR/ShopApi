namespace Shop.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class Basket : BaseEntity {
    public Basket(int userId) {
        UserId = userId;
    }
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual List<BasketItem>? BasketItems { get; set; }
}