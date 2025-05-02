namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Shop.Infrastructure.Data;

public class ProductFilterValueRepository:BaseRepository<ProductFilterValue>,IProductFilterValueRepository {

    public ProductFilterValueRepository(AppDbContext context) : base(context) {
    }
   
}

public interface IProductFilterValueRepository:IRepository<ProductFilterValue> {
}