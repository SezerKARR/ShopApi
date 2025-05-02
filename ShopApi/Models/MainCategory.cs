namespace ShopApi.Models;

using Abstracts;

public class MainCategory:BaseEntity {

    public List<Category> Categories { get; set; } = new List<Category>();
}