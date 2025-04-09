namespace ShopApi.Repository;

using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class BrandRepository:BaseRepository<Brand>,IBrandRepository {

    public BrandRepository(AppDbContext context) : base(context) {
    }
    protected override IQueryable<Brand> Include() {
        return _dbSet.Include(brand => brand.Products);
    }
}

public interface IBrandRepository:IRepository<Brand> {
    
}