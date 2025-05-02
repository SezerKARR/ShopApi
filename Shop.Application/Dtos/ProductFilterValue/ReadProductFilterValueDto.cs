namespace Shop.Application.Dtos.ProductFilterValue;

using System.ComponentModel.DataAnnotations;
using Domain.Models;

public class ReadProductFilterValueDto {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int FilterValueId { get; set; }
    [MaxLength(100)]
    public string? CustomValue { get; set; }
    public int FilterId { get; set; }
    public Filter? Filter { get; set; }
    public FilterValue? FilterValue { get; set; }
}