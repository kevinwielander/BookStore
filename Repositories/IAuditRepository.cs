using BookStore.Data;
using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Repositories;

public interface IAuditRepository
{
    Task AddAuditLogAsync(BookAuditLog auditLog);
    Task<PagedResultDto<BookLogDto>> GetAuditLogsAsync(BookLogQueryParamsDto queryParameters);
}