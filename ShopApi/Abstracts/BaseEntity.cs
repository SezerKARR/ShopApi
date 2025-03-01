namespace ShopApi.Abstracts;

using Interface;

public class BaseEntity:IEntityDate {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    // public DateTime Created { get; set; } = DateTime.UtcNow;
    // public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    // public DateTime LastModified { get; set; } = DateTime.UtcNow;
    // public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
}