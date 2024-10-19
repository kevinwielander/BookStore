using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Services;

public interface IBookService
{
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(string isbn);
    Task<BookDto> AddBookAsync(BookDto bookDto);
    Task UpdateBookAsync(BookDto bookDto);
    Task DeleteBookAsync(string isbn);
}