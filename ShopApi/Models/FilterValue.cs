namespace ShopApi.Models;

using Abstracts;

public class FilterValue:BaseEntity {
    public int FilterId { get; set; }
    public required Filter Filter { get; set; }
}