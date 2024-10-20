using BookStore.Data;
using BookStore.DTOs;

namespace BookStore.Services;

public interface IAuditService
{
    Task LogChangeAsync(string isbn, string action, Dictionary<string, object> changes);
    Task<PagedResultDto<BookLogDto>> GetAuditLogsAsync(BookLogQueryParams queryParameters); 
}