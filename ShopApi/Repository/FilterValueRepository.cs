namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class FilterValueRepository:BaseRepository<FilterValue>,IFilterValueRepository{

    public FilterValueRepository(AppDbContext context) : base(context) {
    }
}

public interface IFilterValueRepository:IRepository<FilterValue> {
}