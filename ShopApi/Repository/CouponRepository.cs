namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

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
