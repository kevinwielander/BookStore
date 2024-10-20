using System.Text.Json;
using BookStore.Data;
using BookStore.DTOs;

namespace BookStore.Mappers;

public class BookLogMapper
{
    public static BookLogDto ToDto(BookAuditLog auditLog)
    {
        var changes = JsonSerializer.Deserialize<Dictionary<string, object>>(auditLog.ChangeDetails);
        string description = GenerateDescription(auditLog.Action, changes);

        return new BookLogDto
        {
            Isbn = auditLog.Isbn,
            ChangeTime = auditLog.Timestamp.ToString("o"),
            Action = auditLog.Action,
            Description = description
        };
    }

    private static string GenerateDescription(string action, Dictionary<string, object> changes)
    {
        switch (action)
        {
            case "Created":
                return "Book was created";
            case "Updated":
                return string.Join(", ", changes.Select(c => $"{c.Key} was changed"));
            case "Deleted":
                return "Book was deleted";
            default:
                return "Unknown action";
        }
    }
}