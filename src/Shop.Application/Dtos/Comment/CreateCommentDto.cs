namespace Shop.Application.Dtos.Comment;

using Microsoft.AspNetCore.Http;
using Validations;

public class CreateCommentDto {
   
    public string? Name { get; set; }
    public string? Content { get; set; }
    public int ProductId { get; set; }
    public int OrderItemId { get; set; }
    public int Rating { get; set; }
    [ImageUploadValidation(5)]
    public List<IFormFile>? Images { get; set; }
}