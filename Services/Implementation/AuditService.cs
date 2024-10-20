using BookStore.Data;
using BookStore.DTOs;
using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services.Implementation;

public class AuditService(IAuditRepository auditRepository, ILogger<AuditService> logger) : IAuditService
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
        logger.LogDebug("Audit log: {@auditLog}", auditLog);

        await auditRepository.AddAuditLogAsync(auditLog);
    }

    public async Task<AuditPageResult<BookAuditLog>> GetAuditLogsAsync(BookLogQueryParams queryParameters)
    {
        logger.LogTrace("GetAuditLogsAsync");
        return await auditRepository.GetAuditLogsAsync(queryParameters);
    }
}