using System.Text.Json;
using BookStore.Data;
using BookStore.DTOs;

namespace BookStore.Mappers;

public class BookLogMapper
{
    public static BookLogDto ToDto(BookAuditLog auditLog)
    {
        var changes = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(auditLog.ChangeDetails);
        string description = GenerateDescription(auditLog.Action, changes);

        return new BookLogDto
        {
            Isbn = auditLog.Isbn,
            ChangeTime = auditLog.Timestamp,
            Action = auditLog.Action,
            Description = description
        };
    }

    private static string GenerateDescription(string action, Dictionary<string, JsonElement> changes)
    {
        switch (action)
        {
            case "Created":
                return "Book was created";
            case "Updated":
                return string.Join(", ", changes.Select(c => 
                {
                    var oldValue = FormatJsonElement(c.Value.GetProperty("Old"));
                    var newValue = FormatJsonElement(c.Value.GetProperty("New"));
                    return $"{c.Key} was changed from '{oldValue}' to '{newValue}'";
                }));
            case "Deleted":
                return "Book was deleted";
            default:
                return "Unknown action";
        }
    }

    private static string FormatJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Array => string.Join(", ", element.EnumerateArray().Select(e => e.GetString())),
            _ => element.GetString() ?? string.Empty
        };
    }
}