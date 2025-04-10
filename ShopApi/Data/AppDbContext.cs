namespace ShopApi.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Filter> Filters { get; set; }
    public DbSet<FilterValue> FilterValues { get; set; }
    public DbSet<ProductFilterValue> ProductFilterValues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {

        base.OnModelCreating(builder);
        builder.Entity<ProductFilterValue>().HasOne(pfv=>pfv.Filter).WithMany(f=>f.ProductFilterValues).HasForeignKey(pfv=>pfv.FilterId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ProductFilterValue>().HasOne(pfv=>pfv.Product).WithMany(product => product.FilterValues).HasForeignKey(p => p.ProductId);
        builder.Entity<FilterValue>().HasOne(fv=>fv.Filter).WithMany(filter => filter.Values).HasForeignKey(f => f.FilterId);
        builder.Entity<Filter>().HasOne(f => f.Category).WithMany(category => category.Filters).HasForeignKey(c => c.CategoryId);
        builder.Entity<Category>()
            .HasOne(category => category.ParentCategory)// Üst kategori
            .WithMany(category => category.SubCategories)// Alt kategoriler
            .HasForeignKey(category => category.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<BasketItem>()
            .HasOne<Basket>()
            .WithMany(basket => basket.BasketItems)
            .HasForeignKey(basketItem => basketItem.BasketId);
        builder.Entity<FilterValue>().HasOne<Filter>().WithMany(f => f.Values).HasForeignKey(f => f.FilterId);


        builder.Entity<Product>()
            .HasOne<Category>()
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
        builder.Entity<Product>().HasOne<User>().WithMany(u => u.Products).HasForeignKey(u => u.CreatedByUserId);
    }

}