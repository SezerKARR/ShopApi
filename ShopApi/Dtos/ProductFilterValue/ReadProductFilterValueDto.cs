namespace ShopApi.Dtos.ProductFilterValue;

using System.ComponentModel.DataAnnotations;
using Models;
using Product;

public class ReadProductFilterValueDto {
    public int ProductId { get; set; }
    public int FilterValueId { get; set; }
    [MaxLength(100)]
    public string? CustomValue { get; set; }
}