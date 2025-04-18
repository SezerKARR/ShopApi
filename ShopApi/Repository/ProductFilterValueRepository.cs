namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class ProductFilterValueRepository:BaseRepository<ProductFilterValue>,IProductFilterValueRepository {

    public ProductFilterValueRepository(AppDbContext context) : base(context) {
    }
   
}

public interface IProductFilterValueRepository:IRepository<ProductFilterValue> {
}