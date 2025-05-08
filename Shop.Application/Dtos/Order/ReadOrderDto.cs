namespace Shop.Application.Dtos.Order;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using OrderItem;
using User;

public class ReadOrderDto {
    public int UserId { get; set; }
    public virtual ReadUserDto User { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public int ShippingAddressId { get; set; }
    public virtual Address ShippingAddress { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? PaymentIntentId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }


    public virtual ICollection<ReadOrderItemDto> OrderItems { get; set; } = new List<ReadOrderItemDto>();

    [MaxLength(50)]
    public string? OrderNumber { get; set; }//todo:bunu yapılandır seller ürün ve user ile bişeyler yaparsın ö.d.
}
