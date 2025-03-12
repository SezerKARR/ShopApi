
namespace ShopApi.Repository;

using Interface;
using Data;
using Models;

public interface IProductRepository : IRepository<Product>{
}
public class ProductRepository : BaseRepository<Product>, IProductRepository {
    public ProductRepository(AppDbContext context) : base(context) {
    }
    

    

}