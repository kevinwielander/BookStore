using BookStore.Data;
using BookStore.DTOs;
using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services.Implementation;

public class AuditService(IAuditRepository auditRepository) : IAuditService
{
    public async Task LogChangeAsync(string isbn, string action, Dictionary<string, object> changes)
    {
        var auditLog = new BookAuditLog
        {
            Isbn = isbn,
            Timestamp = DateTime.UtcNow,
            Action = action,
            Changes = changes
        };

        await auditRepository.AddAuditLogAsync(auditLog);
    }

    public async Task<PagedResultDto<BookLogDto>> GetAuditLogsAsync(BookLogQueryParams queryParameters)
    {
        return await auditRepository.GetAuditLogsAsync(queryParameters);
    }
}