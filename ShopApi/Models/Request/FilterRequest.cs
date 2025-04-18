namespace ShopApi.Models.Request {
	public class FilterRequest
	{
		public int? CategoryId { get; set; }
		public Dictionary<int, List<int>>? GroupedFilterValues { get; set; }
		public int? BrandId { get; set; }  
		public string? SearchQuery { get; set; }
		public string? SortBy { get; set; }
		public bool Ascending { get; set; } = true;
	}

}