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
        
        if (!string.IsNullOrEmpty(queryParameters.FilterKey) && !string.IsNullOrEmpty(queryParameters.FilterValue))
        {
            var filterValue = queryParameters.FilterValue.ToLower();
            query = queryParameters.FilterKey.ToLower() switch
            {
                "isbn" => query.Where(log => log.Isbn.ToLower().Contains(filterValue)),
                "action" => query.Where(log => log.Action.ToLower().Contains(filterValue)),
                "description" => query.Where(log => log.ChangeDetails.ToLower().Contains(filterValue)),
                _ => query
            };
        }
        
        if (!string.IsNullOrEmpty(queryParameters.OrderKey))
        {
            query = queryParameters.OrderKey.ToLower() switch
            {
                "changeTime" => queryParameters.IsDescending ?? true
                    ? query.OrderByDescending(log => log.Timestamp)
                    : query.OrderBy(log => log.Timestamp),
                "isbn" => queryParameters.IsDescending ?? true
                    ? query.OrderByDescending(log => log.Isbn)
                    : query.OrderBy(log => log.Isbn),
                "action" => queryParameters.IsDescending ?? true
                    ? query.OrderByDescending(log => log.Action)
                    : query.OrderBy(log => log.Action),
                _ => query.OrderByDescending(log => log.Timestamp)
            };
        }
        else
        {
            query = query.OrderByDescending(log => log.Timestamp);
        }

        var totalCount = await query.CountAsync();
        
        var auditLogs = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        var bookLogDtos = auditLogs.Select(BookLogMapper.ToDto).ToList();
        
        if (!string.IsNullOrEmpty(queryParameters.GroupByKey))
        {
            bookLogDtos = queryParameters.GroupByKey.ToLower() switch
            {
                "action" => bookLogDtos.GroupBy(log => log.Action).SelectMany(g => g).ToList(),
                _ => bookLogDtos.GroupBy(log => log.ChangeTime.Date).SelectMany(g => g).ToList()
            };
        }

        return new PagedResultDto<BookLogDto>
        {
            Items = bookLogDtos,
            TotalCount = totalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };
    }
}