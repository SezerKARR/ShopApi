namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Comment: BaseEntity {
    public int? UserId{get;set;}
    public User? User{get;set;}
    public DateTime CreatedAt { get; set; }
    [Required]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public int OrderItemId { get; set; } 
    public virtual OrderItem OrderItem { get; set; } // 
    
   [MaxLength(300)]
    public string? Content { get; set; }
    public List<string>? ImageUrls { get; set; }
    
    public int Rating { get; set; }
}