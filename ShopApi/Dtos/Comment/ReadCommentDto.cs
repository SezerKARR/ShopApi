namespace ShopApi.Dtos.Comment;

using Models;
using User;

public class ReadCommentDto {
   
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? UserId{get;set;}
    public ReadUserDto? User{get;set;}
    public DateTime CreatedAt { get; set; }
    public string? Content { get; set; }
    public int ProductSellerId { get; set; }
    public List<string>? ImageUrls { get; set; }
    public int Rating { get; set; }
}