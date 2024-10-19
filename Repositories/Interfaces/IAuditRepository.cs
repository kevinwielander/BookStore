using BookStore.Data;
using BookStore.DTOs;

namespace BookStore.Repositories;

public interface IAuditRepository
{
    Task AddAuditLogAsync(BookAuditLog auditLog);
    Task<PagedResultDto<BookAuditLog>> GetAuditLogsAsync(BookAuditQueryParamsDto queryParameters);
}