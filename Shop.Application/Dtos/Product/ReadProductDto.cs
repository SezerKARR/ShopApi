namespace Shop.Application.Dtos.Product;

using Comment;
using ProductFilterValue;
using ProductImage;
using ProductSeller;

public class ReadProductDto {

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public List<ReadProductImageDto> ProductImages { get; set; } = new List<ReadProductImageDto>();
    public decimal? MinPrice { get; set; }
    public int? MinPriceSellerId { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public required int CreatedByUserId { get; set; }
    public int? BrandId { get; set; }
    public double? AverageRating { get; set; }
    public int CommentCount { get; set; }
    public List<ReadCommentDto> Comments { get; set; } = new List<ReadCommentDto>();
    public List<int>? FilterValueIds { get; set; } = new List<int>();
    public List<int>? ProductSellerIds { get; set; } = new List<int>();
    public List<ReadProductFilterValueDto> FilterValues { get; set; } = new List<ReadProductFilterValueDto>();
    
    
    public List<ReadProductSellerDto> ProductSellers { get; set; } = new List<ReadProductSellerDto>();
    public string? BrandName { get; set; }
}