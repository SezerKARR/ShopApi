namespace ShopApi.Models.Request {
	public class ProductFilterRequest
	{
		public int? CategoryId { get; set; }
		public Dictionary<int, List<int>>? GroupedFilterValues { get; set; }
		public int[]? BrandIds { get; set; }  
		public int MinPrice { get; set; }
		public int MaxPrice { get; set; }
		public string? SearchQuery { get; set; }
		public string? SortBy { get; set; }
		public bool Ascending { get; set; } = true;
	}

}