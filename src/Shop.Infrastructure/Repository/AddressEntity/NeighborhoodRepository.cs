namespace Shop.Infrastructure.Repository.AddressEntity;

using Data;
using Domain.Models.AddressEntities;

public interface INeighborhoodRepository:IRepository<Neighbourhood> {
}

public class NeighborhoodRepository:BaseRepository<Neighbourhood>, INeighborhoodRepository {

    public NeighborhoodRepository(AppDbContext context):base(context) {
    }
}
