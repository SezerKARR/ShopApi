namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Shop.Infrastructure.Data;

public class FilterValueRepository:BaseRepository<FilterValue>,IFilterValueRepository{

    public FilterValueRepository(AppDbContext context) : base(context) {
    }
}

public interface IFilterValueRepository:IRepository<FilterValue> {
}