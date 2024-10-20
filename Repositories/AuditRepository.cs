using BookStore.Data;
using BookStore.DTOs;
using BookStore.Mappers;
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

    public async Task<PagedResultDto<BookLogDto>> GetAuditLogsAsync(BookLogQueryParamsDto queryParameters)
    {
        var query = _context.AuditLogs.AsQueryable();
    
        if (queryParameters.Filters != null && queryParameters.Filters.TryGetValue("Isbn", out var isbn))
        {
            query = query.Where(log => log.Isbn == isbn);
        }
    
        if (!string.IsNullOrEmpty(queryParameters.OrderBy))
        {
            query = queryParameters.OrderBy.ToLower() switch
            {
                "timestamp" => queryParameters.IsDescending ?? true
                    ? query.OrderByDescending(log => log.Timestamp) 
                    : query.OrderBy(log => log.Timestamp),
                _ => query.OrderByDescending(log => log.Timestamp)
            };
        }
        else
        {
            query = query.OrderByDescending(log => log.Timestamp);
        }

        var totalCount = await query.CountAsync();

        var pageNumber = queryParameters.PageNumber ?? 1;
        var pageSize = queryParameters.PageSize ?? 10;

        var auditLogs = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var bookLogDtos = auditLogs.Select(BookLogMapper.ToDto).ToList();

        return new PagedResultDto<BookLogDto>
        {
            Items = bookLogDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}