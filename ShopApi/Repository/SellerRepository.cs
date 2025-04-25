namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class SellerRepository:BaseRepository<Seller>, ISellerRepository {

    public SellerRepository(AppDbContext context):base(context) {
    }
}

public interface ISellerRepository:IRepository<Seller> {
}