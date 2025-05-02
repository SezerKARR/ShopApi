namespace ShopApi.Models;

using Abstracts;

public enum FilterType
{
    None=0,
    Text = 1,     
    Select = 2,     
    Range = 3,      
}

public class Filter:BaseEntity {
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public FilterType? Type { get; set; }
    // public Type? Type { get; set; }  
    public List<FilterValue> Values { get; set; } = new List<FilterValue>();
    public bool IsRequired { get; set; }
}