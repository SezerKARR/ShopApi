
namespace ShopApi.Repository;

using Interface;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public interface IProductRepository : IRepository<Product>{
}
public class ProductRepository : BaseRepository<Product>, IProductRepository {
    public ProductRepository(AppDbContext context) : base(context) {
    }
    
    protected override IQueryable<Product> Include() {
        Console.WriteLine("CategoryRepository");
        return _dbSet.Include(p=>p.FilterValues).AsQueryable();
    }
    

}