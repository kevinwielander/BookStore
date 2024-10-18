using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Services;

public interface IBookService
{
    Task<List<BookDTO>> GetAllBooksAsync();
    Task<BookDTO> GetBookByIdAsync(int id);
    Task<BookDTO> AddBookAsync(BookDTO bookDto);
    Task UpdateBookAsync(BookDTO bookDto);
    Task DeleteBookAsync(int id);
}