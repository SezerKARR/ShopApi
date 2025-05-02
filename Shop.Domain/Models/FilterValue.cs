namespace Shop.Domain.Models;

public class FilterValue:BaseEntity {
    public int FilterId { get; set; }
    public required Filter Filter { get; set; }
}