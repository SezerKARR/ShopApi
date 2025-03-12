namespace ShopApi.Dtos.Comment;

using Abstracts;
public class ReadCommentDto :ReadDto{
    public ReadCommentDto(int ıd) {
        Id = ıd;
    }
    public int Id { get; set; }
}