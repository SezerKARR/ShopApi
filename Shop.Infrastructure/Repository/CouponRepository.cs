namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Data;

public class CouponRepository:BaseRepository<Coupon>,ICouponRepository {

    public CouponRepository(AppDbContext context):base(context) {
        
    }

    public async Task<List<Coupon>> GetCouponBySellerIdAsync(int sellerId, int includes) {
        var query=IncludeQuery(includes).Where(c=>c.SellerId==sellerId);
        return await query.ToListAsync();
        
    }
}

public interface ICouponRepository:IRepository<Coupon> {
    Task<List<Coupon>> GetCouponBySellerIdAsync(int sellerId, int includes);
}
