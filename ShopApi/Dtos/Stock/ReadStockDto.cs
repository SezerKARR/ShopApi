namespace ShopApi.Dtos.Stock;

using ProductSeller;

public class ReadStockDto {

    public  int? ProductSellerId { get; set; }
    public DateTime DateTime { get; set; }
    public int Quantity { get; set; }
}