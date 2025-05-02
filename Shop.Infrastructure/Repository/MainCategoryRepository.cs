namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Shop.Infrastructure.Data;

public interface IMainCategoryRepository:IRepository<MainCategory> {
}
public class MainCategoryRepository:BaseRepository<MainCategory>,IMainCategoryRepository {

    public MainCategoryRepository(AppDbContext context) : base(context) {
    }
}

