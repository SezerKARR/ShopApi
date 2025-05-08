namespace Shop.Infrastructure.Repository;

using Data;
using Domain.Models;

public class OrderRepository:BaseRepository<Order>,IOrderRepository {

    public OrderRepository(AppDbContext context):base(context) {
    }
}

public interface IOrderRepository:IRepository<Order> {
}
