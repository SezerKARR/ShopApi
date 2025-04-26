namespace ShopApi.Expired;

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class ExpiredEntitiesCleanupService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ExpiredEntitiesCleanupService> _logger;

    public ExpiredEntitiesCleanupService(
        AppDbContext dbContext, 
        ILogger<ExpiredEntitiesCleanupService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task CleanupExpiredEntitiesAsync()
    {
        await CleanupExpiredEntitiesOfTypeAsync<Coupon>();
    }

    private async Task CleanupExpiredEntitiesOfTypeAsync<T>() where T : class, IExpirable
    {
        var now = DateTime.UtcNow;
        var expiredEntities = await _dbContext.Set<T>()
            .Where(e => e.ValidUntil < now)
            .ToListAsync();

        if (expiredEntities.Any())
        {
            _dbContext.Set<T>().RemoveRange(expiredEntities);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Removed {expiredEntities.Count} expired {typeof(T).Name} entities");
        }
    }
}