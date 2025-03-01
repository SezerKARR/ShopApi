namespace SaplingStore.Helpers;

public class QueryObject
{
    public string? FilterBy { get; set; } = null;
    public string? SortBy { get; set; } = null;
    public bool IsDecSending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}