namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Comment: BaseEntity {
   [MaxLength(300)]
    public string? Content { get; set; }
    public List<string>? ImageUrls { get; set; }
}