namespace BookStore.DTOs;

public class PagedResultDto<T>
{
    public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
    public IEnumerable<T> Items { get; set; }
    public string OrderBy { get; set; } = "Timestamp";
    public bool IsDescending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
}