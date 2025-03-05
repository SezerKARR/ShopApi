namespace ShopApi.Repository;

using Abstracts;
using AutoMapper;
using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class ProductRepository : BaseRepository<Product>, IProductRepository {
    public ProductRepository(AppDbContext context) : base(context) {
    }
    

    

}