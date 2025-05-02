namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public interface ICommentRepository:IRepository<Comment> {
    Task<(int Sum, int Count)> GetSumAndCountByProductIdAsync(int productId);
}
public class CommentRepository:BaseRepository<Comment>,ICommentRepository {
    public CommentRepository(AppDbContext context) : base(context) {
    }
    public async Task<(int Sum, int Count)> GetSumAndCountByProductIdAsync(int productId)
    {
        var result = await _context.Comments
            .Where(c => c.ProductSeller != null && c.ProductSeller.ProductId == productId)  
            .Select(c => new {
                Sum = c.Rating,
                Count = 1
            })
            .ToListAsync();
       
        int sum = result.Sum(r => r.Sum);  
        int count = result.Count;  
        Console.WriteLine($"Sum: {sum}, Count: {count}");
        return (sum, count);
    }


}

