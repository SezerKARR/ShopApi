namespace ShopApi.Models;

using Abstracts;

public class Comment: BaseEntity {
    public Comment(int id, string? name, string slug, string content, List<string> imageUrls) : base(id, name, slug) {
        Content = content;
        ImageUrls = imageUrls;
    }
    public string Content { get; set; }
    public List<string> ImageUrls { get; set; }
}