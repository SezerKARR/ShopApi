namespace ShopApi.Dtos.ProductFilterValue;

public class CreateProductValueDto {
    public int ProductId { get; set; }
    public int FilterId { get; set; }
    public string? Value { get; set; }
}