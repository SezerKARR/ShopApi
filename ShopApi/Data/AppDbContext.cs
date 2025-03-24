namespace ShopApi.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<MainCategory> MainCategories { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<CategoryTree> CategoryTrees { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      
        base.OnModelCreating(builder);
        builder.Entity<CategoryTree>()
            .HasKey(ct => new { ct.AncestorId, ct.DescendantId });
        builder.Entity<CategoryTree>()
            .HasOne(ct => ct.Ancestor)
            .WithMany()
            .HasForeignKey(ct => ct.AncestorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CategoryTree>()
            .HasOne(ct => ct.Descendant)
            .WithMany()
            .HasForeignKey(ct => ct.DescendantId)
            .OnDelete(DeleteBehavior.Cascade);
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