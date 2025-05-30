namespace Shop.Application.Dtos.Comment;

using User;

public class ReadCommentDto {
   
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? UserId{get;set;}
    public ReadUserDto? User{get;set;}
    public string? Content { get; set; }
    public int ProductSellerId { get; set; }
    public List<string>? ImageUrls { get; set; }
    public int Rating { get; set; }
}