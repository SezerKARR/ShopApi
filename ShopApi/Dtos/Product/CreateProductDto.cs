namespace ShopApi.Dtos.Product;

public class CreateProductDto {

    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
}