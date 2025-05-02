namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

[Flags]
enum SellerIncludes {
    None = 0,
    Coupons=16,
}
public class SellerRepository:BaseRepository<Seller>, ISellerRepository {

    public SellerRepository(AppDbContext context):base(context) {
    }
}

public interface ISellerRepository:IRepository<Seller> {
}