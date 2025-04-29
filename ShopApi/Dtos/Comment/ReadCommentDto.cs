namespace ShopApi.Dtos.Comment;

public class ReadCommentDto {
   
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public int ProductId { get; set; }
    public List<string>? ImageUrls { get; set; }
}