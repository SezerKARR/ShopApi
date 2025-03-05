namespace ShopApi.Repository;

using Abstracts;
using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Common;

public class CategoryRepository :BaseRepository<Category>,ICategoryRepository{
    
    public CategoryRepository(AppDbContext context) : base(context) {
        
    }
    protected override IQueryable<Category> Include(IQueryable<Category> queryable) {
        return queryable.Include(c => c.Products);
    }
    

}