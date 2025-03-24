namespace ShopApi.Models;

public enum Type {
    None,
    Select,
    Range,
    
}
public class Filter {
    public int Id { get; set; }
    public int CategoryId { get; set; }

    public string Name { get; set; } 
    // public Type? Type { get; set; }  
    public List<FilterValue> Options { get; set; }
}