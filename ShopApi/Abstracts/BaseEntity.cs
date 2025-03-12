namespace ShopApi.Abstracts;

using Interface;

public class BaseEntity:IEntityDate,IEntity {
   
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
}