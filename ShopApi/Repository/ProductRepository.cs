namespace ShopApi.Repository;

using AutoMapper;
using Data;
using Interface;
using Microsoft.EntityFrameworkCore;
using Models;

public class ProductRepository(IMapper mapper, AppDbContext context):IProductRepository {
    
    public IQueryable<Product> GetProductsQueryable()
    {
        return context.Products.AsQueryable();
    }
    
    public async Task<List<Product>> GetProductsAsync() {
        List<Product> products = await context.Products.ToListAsync();
       return products;
    }
    public async Task<Product?> GetProductByIdAsync(int id) {
       
        Product product =await context.Products.Include(p=>p.Category).FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }
    public async Task<Product?> GetProductBySlugAsync(string slug) {
        
        Product product = await context.Products.Include(p=>p.Category).FirstOrDefaultAsync(p => p.Slug == slug);
        return product;
    }
    public async Task CreateProductAsync(Product product) {

        await context.Products.AddAsync(product);
    }
    public  void UpdateProduct(Product product) {
        context.Entry(product).State = EntityState.Modified;
    }
    public void DeleteProduct(Product product) {
        context.Products.Remove(product); 
    }
   
}