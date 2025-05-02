namespace Shop.Domain.Models;

public class ProductImage:BaseEntity {
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int ImageId { get; set; }
    public virtual Image? Image { get; set; }
    public int Order { get; set; } = 0; 

}
