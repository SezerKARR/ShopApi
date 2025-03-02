namespace ShopApi.Data;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Models;
public interface IUnitOfWork {
    Task<bool> CommitAsync();
}
public class UnitOfWork(AppDbContext context, IMapper mapper ) :IUnitOfWork{

    public async Task<bool> CommitAsync() {
        return await context.SaveChangesAsync() > 0;
    }
}

