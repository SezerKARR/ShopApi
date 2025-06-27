namespace Shop.Infrastructure.Repository;

using Data;
using Domain.Models;

public interface IOrderItemRepository: IRepository<OrderItem> {
}

public class OrderItemRepository:BaseRepository<OrderItem>, IOrderItemRepository {

    public OrderItemRepository(AppDbContext context):base(context) {
    }
}
