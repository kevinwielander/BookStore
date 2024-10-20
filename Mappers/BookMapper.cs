using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Mappers;

public class BookMapper
{
    public static BookDto ToDto(Book book)
    {
        return new BookDto
        {
            Isbn = book.Isbn,
            Title = book.Title,
            Description = book.Description,
            PublishDate = book.PublishDate,
            Authors = book.Authors,
        };
    }

    public static Book ToModel(BookDto bookDto)
    {
        return new Book
        {
            Isbn = bookDto.Isbn,
            Title = bookDto.Title,
            Description = bookDto.Description,
            PublishDate = bookDto.PublishDate,
            Authors = bookDto.Authors,
            RowVersion = new byte[8]
        };
    }
}