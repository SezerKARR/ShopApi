namespace Shop.Infrastructure.Repository.AddressEntity;

using Data;
using Domain.Models.AddressEntities;

public interface IDistrictRepository:IRepository<District> {
}

public class DistrictRepository:BaseRepository<District>, IDistrictRepository {

    public DistrictRepository(AppDbContext context):base(context) {
    }
}
