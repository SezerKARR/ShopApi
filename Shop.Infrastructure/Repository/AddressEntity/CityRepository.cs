namespace Shop.Infrastructure.Repository.AddressEntity;

using Data;
using Domain.Models.AddressEntities;

public interface ICityRepository: IRepository<City>{
}

public class CityRepository:BaseRepository<City>, ICityRepository {

    public CityRepository(AppDbContext context):base(context) {
    }
}
