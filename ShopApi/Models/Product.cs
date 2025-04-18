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
    public decimal Price { get; set; }

    public int BrandId { get; set; }
    public  Brand? Brand { get; set; }
    public required int CreatedByUserId { get; set; }
    public  User? CreatedByUser { get; set; }
    public List<ProductSeller> ProductSellers { get; set; } = new List<ProductSeller>();
    public List<ProductFilterValue> FilterValues { get; set; } = new List<ProductFilterValue>();

    
    

}