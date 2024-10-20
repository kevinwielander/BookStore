using BookStore.DTOs;
using BookStore.Exceptions;
using BookStore.Repositories;

namespace BookStore.Services.Implementation;

public class BookService(IBookRepository bookRepository, ILogger<BookService> logger, IAuditService auditService)
    : IBookService
{
    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        try
        {
            logger.LogTrace("Retrieving all books");
            var books = await bookRepository.GetBooksAsync();
            return [..books];
        }
        catch (Exception ex)
        {
            logger.LogError("Error retrieving books!");
            throw new ApplicationException("Error occurred while fetching all books", ex);
        }
    }

    public async Task<BookDto> GetBookByIdAsync(string isbn)
    {
        try
        {
            logger.LogTrace("Retrieving book with isbn {isbn}", isbn);
            var book = await bookRepository.GetBookByIdAsync(isbn);
            return book;
        }
        catch (KeyNotFoundException)
        { 
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("Error retrieving book with isbn {isbn}", isbn);
            throw new ApplicationException($"Error occurred while fetching book with isbn {isbn}", ex);
        }
    }

    public async Task<BookDto> AddBookAsync(BookDto bookDto)
    {
        try
        {
            logger.LogTrace("Adding book: {@bookDto}",bookDto);
            await bookRepository.AddBookAsync(bookDto);
            await auditService.LogChangeAsync(bookDto.Isbn, "Created", new Dictionary<string, object>
            {
                { "Title", bookDto.Title },
                { "Description", bookDto.Description },
                { "PublishDate", bookDto.PublishDate },
                { "Authors", bookDto.Authors }
            });
            return bookDto;
        }
        catch (BookAlreadyExistsException ex)
        {
            throw;
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
            logger.LogTrace("Updating book: {@bookDto}", bookDto);
            var originalBook = await bookRepository.GetBookByIdAsync(bookDto.Isbn);
            await bookRepository.UpdateBookAsync(bookDto);
        
            var changes = new Dictionary<string, object>();
            if (originalBook.Title != bookDto.Title)
                changes["Title"] = new { Old = originalBook.Title, New = bookDto.Title };
            if (originalBook.Description != bookDto.Description)
                changes["Description"] = new { Old = originalBook.Description, New = bookDto.Description };
            if (originalBook.PublishDate != bookDto.PublishDate)
                changes["PublishDate"] = new { Old = originalBook.PublishDate, New = bookDto.PublishDate };
            if (!originalBook.Authors.SequenceEqual(bookDto.Authors))
                changes["Authors"] = new { Old = originalBook.Authors, New = bookDto.Authors };

            if (changes.Count != 0)
            {
                await auditService.LogChangeAsync(bookDto.Isbn, "Updated", changes);
            }
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
            logger.LogTrace("Deleting book with isbn {isbn}", isbn);
            var book = await bookRepository.GetBookByIdAsync(isbn);
            await bookRepository.DeleteBookAsync(isbn);
            await auditService.LogChangeAsync(isbn, "Deleted", new Dictionary<string, object>
            {
                { "Title", book.Title },
                { "Description", book.Description },
                { "PublishDate", book.PublishDate },
                { "Authors", book.Authors }
            });
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error occurred while deleting book with isbn {isbn}", ex);
        }
    }
}