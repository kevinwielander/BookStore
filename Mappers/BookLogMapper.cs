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
            ChangeTime = auditLog.Timestamp,
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
                return string.Join(", ", changes.Select(c => 
                {
                    var change = JsonSerializer.Deserialize<Dictionary<string, string>>(c.Value.ToString());
                    return $"{c.Key} was changed from '{change["Old"]}' to '{change["New"]}'";
                }));
            case "Deleted":
                return "Book was deleted";
            default:
                return "Unknown action";
        }
    }
}