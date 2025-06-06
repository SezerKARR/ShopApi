namespace Shop.Application.Dtos.ProductFilterValue;

public class CreateProductValueDto {
    public int ProductId { get; set; }
    public int FilterId { get; set; }
    public int FilterValueId { get; set; } = -1;
    public string? CustomValue { get; set; }
}