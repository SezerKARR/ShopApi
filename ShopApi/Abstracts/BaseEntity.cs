namespace ShopApi.Abstracts;

using Interface;

public class BaseEntity:IEntityDate,IEntity {
    public BaseEntity(int id, string? name, string slug) {
        Id = id;
        Name = name;
        Slug = slug;
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Slug { get; set; }
}