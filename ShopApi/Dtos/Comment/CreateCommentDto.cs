namespace ShopApi.Dtos.Comment;

using Validations;

public class CreateCommentDto {
   
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public int ProductId { get; set; }
    public int Rating { get; set; }
    [ImageUploadValidation(5)]
    public List<IFormFile>? Images { get; set; }
}