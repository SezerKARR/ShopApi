namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class FilterRepository:BaseRepository<Filter>,IFilterRepository{

    public FilterRepository(AppDbContext context) : base(context) {
    }
}

public interface IFilterRepository:IRepository<Filter> {
}