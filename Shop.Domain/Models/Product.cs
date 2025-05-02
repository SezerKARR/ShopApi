namespace Shop.Domain.Models;

public class Product : BaseEntity {


    public string? Description { get; set; }

    public required int CategoryId { get; set; }
    public  Category? Category { get; set; }

    public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    
    public decimal? MinPrice { get; set; }
    public int? MinPriceSellerId { get; set; }
        
    public int BrandId { get; set; }
    public  Brand? Brand { get; set; }
    public required int CreatedBySellerId { get; set; }
    public  Seller? CreatedBySeller { get; set; }
    public int CommentCount { get; set; }

    public double? AverageRating { get; set; }
    public List<ProductSeller> ProductSellers { get; set; } = new List<ProductSeller>();
    public List<ProductFilterValue> FilterValues { get; set; } = new List<ProductFilterValue>();
    public List<Comment>? Comments { get; set; } = new List<Comment>();




}