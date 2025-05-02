namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Shop.Infrastructure.Data;

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