namespace ShopApi.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Product> Products { get; set; }
    public DbSet<Category?> Categories { get; set; }
    // builder.Entity<SaplingHeight>()
    // .HasOne(s => s.Sapling)
    //     .WithMany(h => h.SaplingHeights)
    //     .HasForeignKey(s => s.SaplingId);
    //
    // builder.Entity<Sapling>()
    // .HasOne(s => s.SaplingCategory)
    //     .WithMany(c => c.Saplings)
    //     .HasForeignKey(s => s.SaplingCategoryId);
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasOne<Category>() // Navigation property yok ama Foreign Key korundu
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }

}