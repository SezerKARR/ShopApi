
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace ShopApi.Interface;

using Abstracts;
using Data;
using Models;

public interface IProductRepository : IRepository<Product>{
}
public class ProductRepository : BaseRepository<Product>, IProductRepository {
    public ProductRepository(AppDbContext context) : base(context) {
    }
    

    

}