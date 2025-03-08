namespace ShopApi.Repository;

using Abstracts;
using Data;
using Interface;
using Models;

public interface ICommentRepository:IRepository<Comment> {
}
public class CommentRepository:BaseRepository<Comment>,ICommentRepository {
    public CommentRepository(AppDbContext context) : base(context) {
    }
}

