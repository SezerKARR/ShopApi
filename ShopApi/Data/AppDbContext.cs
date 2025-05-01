namespace ShopApi.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<BrandCategory> BrandCategories { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Filter> Filters { get; set; }
    public DbSet<FilterValue> FilterValues { get; set; }
    public DbSet<ProductFilterValue> ProductFilterValues { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<ProductSeller> ProductSellers { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder) {

        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasDiscriminator<int>("RoleInt")  // "Role" kolonu discriminator olacak
            .HasValue<User>(1)      // "User" tipi için Role.User
            .HasValue<Seller>(2);

        builder.Entity<Coupon>().HasOne(coupon => coupon.Seller).WithMany(s=>s.Coupons).HasForeignKey(coupon => coupon.SellerId);

        builder.Entity<BrandCategory>().HasOne(bc=>bc.Category).WithMany(c=>c.BrandCategories).HasForeignKey(bc=>bc.CategoryId);
        builder.Entity<BrandCategory>().HasOne(bc => bc.Brand).WithMany(c=>c.BrandCategories).HasForeignKey(bc=>bc.BrandId);
        builder.Entity<ProductFilterValue>().HasOne(pfv => pfv.Product).WithMany(product => product.FilterValues).HasForeignKey(p => p.ProductId);
        builder.Entity<FilterValue>().HasOne(fv => fv.Filter).WithMany(filter => filter.Values).HasForeignKey(f => f.FilterId);
        builder.Entity<Filter>().HasOne(f => f.Category).WithMany(category => category.Filters).HasForeignKey(c => c.CategoryId);
        builder.Entity<Category>()
            .HasOne(category => category.ParentCategory)// Üst kategori
            .WithMany(category => category.SubCategories)// Alt kategoriler
            .HasForeignKey(category => category.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<BasketItem>()
            .HasOne(bi => bi.Basket)
            .WithMany(basket => basket.BasketItems)
            .HasForeignKey(basketItem => basketItem.BasketId);
        builder.Entity<FilterValue>().HasOne(fv => fv.Filter).WithMany(f => f.Values).HasForeignKey(fv => fv.FilterId);

        builder.Entity<Stock>().HasOne(s=>s.ProductSeller).WithMany(ps=>ps.Stocks).HasForeignKey(s=>s.ProductSellerId);
        
        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
        builder.Entity<ProductSeller>().HasOne(ps=>ps.Seller).WithMany(seller =>seller.ProductSellers ).HasForeignKey(ps=>ps.SellerId);
        builder.Entity<ProductSeller>().HasOne(ps=>ps.Product).WithMany(product => product.ProductSellers).HasForeignKey(ps=>ps.ProductId);
        builder.Entity<Product>()
            .HasOne(p => p.CreatedBySeller)
            .WithMany(u => u.CreatedProducts)
            .HasForeignKey(p => p.CreatedBySellerId);
        builder.Entity<Comment>().HasOne(c=>c.ProductSeller).WithMany(ps => ps.Comments).HasForeignKey(c=>c.ProductSellerId);
        builder.Entity<Comment>().HasOne(c=>c.User).WithMany(u=>u.Comments).HasForeignKey(c=>c.UserId);

        builder.Entity<ProductImage>().HasOne(pi => pi.Product).WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId);


    }
}