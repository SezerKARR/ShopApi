namespace ShopApi.Models;

using Abstracts;

public enum Type {
    None,
    Select,
    Range,
    
}
public class Filter:BaseEntity {
    public int CategoryId { get; set; }

    // public Type? Type { get; set; }  
    public List<FilterValue>? Values { get; set; } = new List<FilterValue>();
}