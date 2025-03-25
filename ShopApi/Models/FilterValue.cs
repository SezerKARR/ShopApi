namespace ShopApi.Models;

using Abstracts;

public class FilterValue:BaseEntity {
    public string? Value { get; set; }
    public int? FilterId { get; set; }
}