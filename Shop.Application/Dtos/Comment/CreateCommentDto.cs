namespace Shop.Application.Dtos.Comment;

using Microsoft.AspNetCore.Http;
using Validations;

public class CreateCommentDto {
   
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public int ProductSellerId { get; set; }
    public int Rating { get; set; }
    [ImageUploadValidation(5)]
    public List<IFormFile>? Images { get; set; }
}