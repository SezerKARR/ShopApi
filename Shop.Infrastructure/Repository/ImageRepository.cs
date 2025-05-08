namespace Shop.Infrastructure.Repository;

using Data;
using Domain.Models;

public class ImageRepository:BaseRepository<Image>,IImageRepository {

    public ImageRepository(AppDbContext context):base(context) {
    }
}

public interface IImageRepository:IRepository<Image> {
}
