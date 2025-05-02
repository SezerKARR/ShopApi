namespace ShopApi.Dtos.Stock;

using ProductSeller;

public class CreateStockDto {
    public int ProductSellerId{get;set;}
    public int Quantity { get; set; }

}