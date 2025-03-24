namespace ShopApi.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Filter> Filters { get; set; }
    public DbSet<User> Users { get; set; }
    // public DbSet<CategoryTree> CategoryTrees { get; set; }
    // public DbSet<MainCategory> MainCategories { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
      
        base.OnModelCreating(builder);
        builder.Entity<Filter>().HasOne<Category>().WithMany(category=>category.Filters).HasForeignKey(c=>c.CategoryId);
        builder.Entity<Category>().HasOne<Category>().WithMany(category => category.SubCategories).HasForeignKey(c => c.ParentId);
        builder.Entity<BasketItem>()
            .HasOne<Basket>()
            .WithMany(basket=>basket.BasketItems)
            .HasForeignKey(basketItem => basketItem.BasketId);
        
        // builder.Entity<Category>()
        //     .HasOne<MainCategory>()
        //     .WithMany(mainCategory=>mainCategory.Categories)
        //     .HasForeignKey(c=>c.MainCategoryId);
        
        builder.Entity<Product>()
            .HasOne<Category>() 
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }

}