using BookStore.Data;
using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Services;

public interface IAuditService
{
    Task LogChangeAsync(string isbn, string action, Dictionary<string, object> changes);
    Task<AuditPageResult<BookAuditLog>> GetAuditLogsAsync(BookLogQueryParams queryParameters); 
}