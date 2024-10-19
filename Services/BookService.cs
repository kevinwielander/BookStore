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


    public async Task<List<BookDto>> GetAllBooksAsync()
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

    public async Task<BookDto> GetBookByIdAsync(string isbn)
    {
        try
        {
            var book = await _bookRepository.GetBookByIdAsync(isbn);
            return book;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // exception logging
            throw new ApplicationException($"Error occurred while fetching book with isbn {isbn}", ex);
        }
    }

    public async Task<BookDto> AddBookAsync(BookDto bookDto)
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

    public async Task UpdateBookAsync(BookDto bookDto)
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
            throw new ApplicationException($"Error occurred while updating book with isbn {bookDto.Isbn}", ex);
        }
    }

    public async Task DeleteBookAsync(string isbn)
    {
        try
        {
            await _bookRepository.DeleteBookAsync(isbn);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error occurred while deleting book with isbn {isbn}", ex);
        }
    }
}