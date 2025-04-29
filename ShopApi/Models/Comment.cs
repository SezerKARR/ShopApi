namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Comment: BaseEntity {
    public int? UserId{get;set;}
    public User? User{get;set;}
    public int ProductSellerId{get;set;}
    public ProductSeller? ProductSeller{get;set;}
    
   [MaxLength(300)]
    public string? Content { get; set; }
    public List<string>? ImageUrls { get; set; }
    
    public int Rating { get; set; }
}