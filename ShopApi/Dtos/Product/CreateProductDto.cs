namespace ShopApi.Dtos.Product;

public class CreateProductDto {

    public CreateProductDto(int ıd, string name, int categoryId, IFormFile ımageFile) {
        Id = ıd;
        Name = name;
        CategoryId = categoryId;
        ImageFile = ımageFile;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public IFormFile ImageFile { get; set; }
}