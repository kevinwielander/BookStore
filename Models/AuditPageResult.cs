namespace BookStore.Models;

public class AuditPageResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
}