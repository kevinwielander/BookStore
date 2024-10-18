using BookStore.DTOs;

namespace BookStore.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookDTO>> GetBooksAsync();
    Task<BookDTO> GetBookByIdAsync(int id);
    Task AddBookAsync(BookDTO bookDto);
    Task UpdateBookAsync(BookDTO bookDto);
    Task DeleteBookAsync(int id);
}