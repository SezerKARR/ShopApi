namespace ShopApi.Data;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
public interface IUnitOfWork {
    Task<bool> CommitAsync();
}
public class UnitOfWork(AppDbContext context, IMapper mapper ) :IUnitOfWork{

    public async Task<bool> CommitAsync() {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
        return false;
    }
}

