namespace BookStore.DTOs;

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
}