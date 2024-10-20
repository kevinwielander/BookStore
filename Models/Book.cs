using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public class Book
{
    [Key]
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly PublishDate { get; set; }
    public List<string> Authors { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; }
}