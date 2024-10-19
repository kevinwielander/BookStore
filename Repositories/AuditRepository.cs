using BookStore.Data;
using BookStore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly BookStoreContext _context;

    public AuditRepository(BookStoreContext context)
    {
        _context = context;
    }

    public async Task AddAuditLogAsync(BookAuditLog auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResultDto<BookAuditLog>> GetAuditLogsAsync(BookAuditQueryParamsDto queryParameters)
    {
        var query = _context.AuditLogs.AsQueryable();
        
        if (queryParameters.Filters.TryGetValue("Isbn", out var isbn))
        {
            query = query.Where(log => log.Isbn == isbn);
        }
        
        query = queryParameters.OrderBy.ToLower() switch
        {
            "timestamp" => queryParameters.IsDescending 
                ? query.OrderByDescending(log => log.Timestamp) 
                : query.OrderBy(log => log.Timestamp),
        };

        var totalCount = await query.CountAsync();

        var auditLogs = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        return new PagedResultDto<BookAuditLog>
        {
            Items = auditLogs,
            TotalCount = totalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };
    }
}