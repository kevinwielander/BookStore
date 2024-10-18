using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Mappers;

public class BookMapper
{
    public static BookDTO ToDto(Book book)
    {
        return new BookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            PublishDate = book.PublishDate,
            Authors = book.Authors
        };
    }

    public static Book ToModel(BookDTO bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Description = bookDto.Description,
            PublishDate = bookDto.PublishDate,
            Authors = bookDto.Authors
        };
    }
}