namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class ProductSellerRepository:BaseRepository<ProductSeller>,IProductSellerRepository {

    public ProductSellerRepository(AppDbContext context) : base(context) {
    }
}

public interface IProductSellerRepository:IRepository<ProductSeller> {
}