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
    public async Task<ActionResult<PagedResultDto<BookAuditLog>>> GetAuditBooksAsync(
        [FromQuery] BookLogQueryParamsDto queryParameters)
    {
        try
        {
            logger.LogInformation("Retrieving book audit logs");
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