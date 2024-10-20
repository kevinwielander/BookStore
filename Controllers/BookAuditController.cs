using BookStore.Data;
using BookStore.DTOs;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class BookAuditController(IAuditService auditService, ILogger<BookAuditController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<BookLogDto>>> GetAuditBooksAsync(
        [FromQuery] string? filterKey,
        [FromQuery] string? filterValue,
        [FromQuery] string? orderKey,
        [FromQuery] bool? isDescending,
        [FromQuery] string? groupByKey,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            logger.LogInformation("Retrieving book audit logs");
            var queryParameters = new BookLogQueryParamsDto
            {
                FilterKey = filterKey,
                FilterValue = filterValue,
                OrderKey = orderKey,
                IsDescending = isDescending,
                GroupByKey = groupByKey,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await auditService.GetAuditLogsAsync(queryParameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving book audit logs");
            return StatusCode(500, "An error occurred while retrieving book audit logs");
        }
    }
}