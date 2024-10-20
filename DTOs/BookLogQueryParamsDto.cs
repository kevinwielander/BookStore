namespace BookStore.DTOs;

public class BookLogQueryParamsDto
{
    public Dictionary<string, string>? Filters { get; set; } = new Dictionary<string, string>();
    public string? OrderBy { get; set; } = "Timestamp";
    public bool? IsDescending { get; set; } = true;
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}