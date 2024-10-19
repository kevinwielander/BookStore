namespace BookStore.DTOs;

public class BookDto
{
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly PublishDate { get; set; }
    public List<string> Authors { get; set; }
}