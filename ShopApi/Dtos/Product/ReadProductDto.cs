namespace ShopApi.Dtos.Product;

using Brand;
using Models;
using ProductFilterValue;
using ProductSeller;
using User;

public class ReadProductDto {

    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Slug { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? MinPrice { get; set; }
    public int? MinPriceSellerId { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public required int CreatedByUserId { get; set; }
    public int? BrandId { get; set; }
    public double? AverageRating { get; set; }
    public int CommentCount { get; set; }
    public List<int>? FilterValueIds { get; set; } = new List<int>();
    public List<int>? ProductSellerIds { get; set; } = new List<int>();
    
    public List<ReadProductSellerDto> ProductSellers { get; set; } = new List<ReadProductSellerDto>();
    public string? BrandName { get; set; }
}