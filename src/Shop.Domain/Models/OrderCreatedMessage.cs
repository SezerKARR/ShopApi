namespace Shop.Domain.Models;

public class OrderCreatedMessage
{
    public string Email { get; set; }
    public string OrderId { get; set; }
    public DateTime OrderDate { get; set; }
}
