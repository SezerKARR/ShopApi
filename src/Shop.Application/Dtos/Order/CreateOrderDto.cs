namespace Shop.Application.Dtos.Order;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using OrderItem;
using User;

public class CreateOrderDto {
    public int UserId { get; set; }

    public int ShippingAddressId { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    public string? PaymentIntentId { get; set; }//stripe eklenecek
    public PaymentStatus? PaymentStatus { get; set; }


    public virtual ICollection<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();

    [MaxLength(50)]
    public string? OrderNumber { get; set; }
}
