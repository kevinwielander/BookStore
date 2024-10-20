using BookStore.DTOs;
using BookStore.Exceptions;
using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services.Implementation;

public class BookService(IBookRepository bookRepository, ILogger<BookService> logger, IAuditService auditService)
    : IBookService
{
    public async Task<List<Book>> GetAllBooksAsync()
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

    public async Task<Book> GetBookByIdAsync(string isbn)
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

    public async Task<Book> AddBookAsync(Book book)
    {
        try
        {
            logger.LogTrace("Adding book: {@book}",book);
            await bookRepository.AddBookAsync(book);
            await auditService.LogChangeAsync(book.Isbn, "Created", new Dictionary<string, object>
            {
                { "Title", book.Title },
                { "Description", book.Description },
                { "PublishDate", book.PublishDate },
                { "Authors", book.Authors }
            });
            return book;
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

    public async Task UpdateBookAsync(Book book)
    {
        try
        {
            logger.LogTrace("Updating book: {@book}", book);
            var originalBook = await bookRepository.GetBookByIdAsync(book.Isbn);
            await bookRepository.UpdateBookAsync(book);
        
            var changes = new Dictionary<string, object>();
            if (originalBook.Title != book.Title)
                changes["Title"] = new { Old = originalBook.Title, New = book.Title };
            if (originalBook.Description != book.Description)
                changes["Description"] = new { Old = originalBook.Description, New = book.Description };
            if (originalBook.PublishDate != book.PublishDate)
                changes["PublishDate"] = new { Old = originalBook.PublishDate, New = book.PublishDate };
            if (!originalBook.Authors.SequenceEqual(book.Authors))
                changes["Authors"] = new { Old = originalBook.Authors, New = book.Authors };
            
            logger.LogDebug("Changes detected: {@Changes}", changes);
            if (changes.Count != 0)
            {
                await auditService.LogChangeAsync(book.Isbn, "Updated", changes);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error occurred while updating book with isbn {book.Isbn}", ex);
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