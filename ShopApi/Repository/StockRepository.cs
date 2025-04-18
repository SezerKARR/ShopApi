namespace ShopApi.Repository;

using Data;
using Interface;
using Models;

public class StockRepository:BaseRepository<Stock>,IStockRepository {

    public StockRepository(AppDbContext context) : base(context) {
    }
}

public interface IStockRepository:IRepository<Stock> {
}