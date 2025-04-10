namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class BrandCategoryRepository:BaseRepository<BrandCategory>,IBrandCategoryRepository{

    public BrandCategoryRepository(AppDbContext context) : base(context) {
    }
}

public interface IBrandCategoryRepository:IRepository<BrandCategory> {
}