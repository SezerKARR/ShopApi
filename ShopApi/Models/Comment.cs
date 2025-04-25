namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Comment: BaseEntity {
    public int ProductId { get; set; }
    public Product? Product { get; set; }
   [MaxLength(300)]
    public string? Content { get; set; }
    public List<string>? ImageUrls { get; set; }
    
    public int Rating { get; set; }
}