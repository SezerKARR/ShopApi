namespace ShopApi.Repository;

using Data;
using Interface;
using Models;
public interface IMainCategoryRepository:IRepository<MainCategory> {
}
public class MainCategoryRepository:BaseRepository<MainCategory>,IMainCategoryRepository {

    public MainCategoryRepository(AppDbContext context) : base(context) {
    }
}

