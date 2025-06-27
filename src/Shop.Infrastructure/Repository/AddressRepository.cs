namespace Shop.Infrastructure.Repository;

using Data;
using Domain.Models;

public class AddressRepository:BaseRepository<Address>,IAddressRepository {

    public AddressRepository(AppDbContext context):base(context) {
    }
}

public interface IAddressRepository:IRepository<Address> {
}
