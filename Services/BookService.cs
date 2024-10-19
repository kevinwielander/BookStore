using BookStore.DTOs;
using BookStore.Repositories;

namespace BookStore.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }


    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        try
        {
            _logger.LogTrace("Retrieving all books");
            var books = await _bookRepository.GetBooksAsync();
            return [..books];
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving books!");
            throw new ApplicationException("Error occurred while fetching all books", ex);
        }
    }

    public async Task<BookDto> GetBookByIdAsync(string isbn)
    {
        try
        {
            _logger.LogTrace("Retrieving book with isbn {isbn}", isbn);
            var book = await _bookRepository.GetBookByIdAsync(isbn);
            return book;
        }
        catch (KeyNotFoundException)
        { 
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving book with isbn {isbn}", isbn);
            throw new ApplicationException($"Error occurred while fetching book with isbn {isbn}", ex);
        }
    }

    public async Task<BookDto> AddBookAsync(BookDto bookDto)
    {
        try
        {
            _logger.LogTrace("Adding book: {@bookDto}",bookDto);
            await _bookRepository.AddBookAsync(bookDto);
            return bookDto;
        }
        catch (Exception ex)
        { 
            throw new ApplicationException("Error occurred while adding a new book", ex);
        }
    }

    public async Task UpdateBookAsync(BookDto bookDto)
    {
        try
        {
            _logger.LogTrace("Updating book: {@bookDto}",bookDto);
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
            _logger.LogTrace("Deleting book with isbn {isbn}", isbn);
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