namespace Shop.Domain.Models;

public class MainCategory:BaseEntity {

    public List<Category> Categories { get; set; } = new List<Category>();
}