namespace Shop.Domain.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned
}

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public int ShippingAddressId { get; set; }
    public virtual Address ShippingAddress { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    public string? PaymentIntentId { get; set; }
    public string? PaymentStatus { get; set; }


    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [MaxLength(50)]
    public string? OrderNumber { get; set; }
}