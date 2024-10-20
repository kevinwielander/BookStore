namespace BookStore.DTOs;

public class BookLogQueryParams
{
    public string? FilterKey { get; set; }
    public string? FilterValue { get; set; }
    public string? OrderKey { get; set; }
    public bool? IsDescending { get; set; } = true;
    public string? GroupByKey { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}