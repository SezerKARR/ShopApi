namespace Shop.Infrastructure.Repository;

using Domain.Models;
using Shop.Infrastructure.Data;

public class StockRepository:BaseRepository<Stock>,IStockRepository {

    public StockRepository(AppDbContext context) : base(context) {
    }
}

public interface IStockRepository:IRepository<Stock> {
}