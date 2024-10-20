using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(string isbn);
    Task<Book> AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(string isbn);
}