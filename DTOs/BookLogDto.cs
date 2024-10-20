namespace BookStore.DTOs;

public class BookLogDto
{
    public string Isbn { get; set; }
    public string ChangeTime { get; set; }
    public string Action { get; set; }
    public String Description { get; set; }
}