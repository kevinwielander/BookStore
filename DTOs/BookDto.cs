using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs;

public class BookDto
{
    [Required(ErrorMessage = "ISBN is required")]
    [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters")]
    public string Isbn { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
    public string Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Publish date is required")]
    public DateOnly PublishDate { get; set; }

    [Required(ErrorMessage = "At least one author is required")]
    public List<string> Authors { get; set; }
}