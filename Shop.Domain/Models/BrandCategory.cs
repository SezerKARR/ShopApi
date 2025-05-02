namespace Shop.Domain.Models;

public class BrandCategory :BaseEntity{
    public required int CategoryId { get; set; }
    public required Category Category { get; set; }
    public required int BrandId { get; set; }
    public required Brand Brand { get; set; }
}