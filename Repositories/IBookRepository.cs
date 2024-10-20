using BookStore.DTOs;

namespace BookStore.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookDto>> GetBooksAsync();
    Task<BookDto> GetBookByIdAsync(string isbn);
    Task AddBookAsync(BookDto bookDto);
    Task UpdateBookAsync(BookDto bookDto);
    Task DeleteBookAsync(string isbn);
}