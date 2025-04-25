namespace ShopApi.Models;

using System.ComponentModel.DataAnnotations;
using Abstracts;

public class Product : BaseEntity {


    [MaxLength(180)]
    public string? Description { get; set; }

    public required int CategoryId { get; set; }
    public  Category? Category { get; set; }

    [MaxLength(120)]
    public string? ImageUrl { get; set; }
    
    public decimal? MinPrice { get; set; }
    public int? MinPriceSellerId { get; set; }
        
    public int BrandId { get; set; }
    public  Brand? Brand { get; set; }
    public required int CreatedBySellerId { get; set; }
    public  Seller? CreatedBySeller { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public int CommentCount { get; set; }

    public double? AverageRating { get; set; }
    public List<ProductSeller>? ProductSellers { get; set; } = new List<ProductSeller>();
    public List<ProductFilterValue> FilterValues { get; set; } = new List<ProductFilterValue>();

    
    

}