namespace ShopApi.Dtos.Comment;

public class CreateCommentDto {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Images { get; set; }
}