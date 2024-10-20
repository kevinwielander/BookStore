using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BookStore.Models;

public class BookAuditLog
{
    public string Isbn { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; }
    public string ChangeDetails { get; set; }

    [NotMapped]
    public Dictionary<string, object> Changes
    {
        get => JsonSerializer.Deserialize<Dictionary<string, object>>(ChangeDetails);
        set => ChangeDetails = JsonSerializer.Serialize(value);
    }
}