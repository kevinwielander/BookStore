using BookStore.DTOs;
using BookStore.Repositories;

namespace BookStore.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }


    public async Task<List<BookDTO>> GetAllBooksAsync()
    {
        try
        {
            var books = await _bookRepository.GetBooksAsync();
            return [..books];
        }
        catch (Exception ex)
        {
            // Logging of exception
            throw new ApplicationException("Error occurred while fetching all books", ex);
        }
    }

    public async Task<BookDTO> GetBookByIdAsync(int id)
    {
        try
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            return book;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // exception logging
            throw new ApplicationException($"Error occurred while fetching book with id {id}", ex);
        }
    }

    public async Task<BookDTO> AddBookAsync(BookDTO bookDto)
    {
        try
        {
            await _bookRepository.AddBookAsync(bookDto);
            return bookDto;
        }
        catch (Exception ex)
        {
            // exception logging
            throw new ApplicationException("Error occurred while adding a new book", ex);
        }
    }

    public async Task UpdateBookAsync(BookDTO bookDto)
    {
        try
        {
            await _bookRepository.UpdateBookAsync(bookDto);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error occurred while updating book with id {bookDto.Id}", ex);
        }
    }

    public async Task DeleteBookAsync(int id)
    {
        try
        {
            await _bookRepository.DeleteBookAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error occurred while deleting book with id {id}", ex);
        }
    }
}