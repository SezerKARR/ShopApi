namespace ShopApi.Models;

using Abstracts;

public class Comment: BaseEntity {
    public string Content { get; set; }
    public List<string> ImageUrls { get; set; }
}