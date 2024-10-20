using System.Data;
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
        logger.LogTrace("Retrieving all books");
        var books = await bookRepository.GetBooksAsync();
        return [..books];
    }

    public async Task<Book> GetBookByIdAsync(string isbn)
    {
        logger.LogTrace("Retrieving book with isbn {isbn}", isbn);
        var book = await bookRepository.GetBookByIdAsync(isbn);
        return book;
    }

    public async Task<Book> AddBookAsync(Book book)
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

    public async Task UpdateBookAsync(Book book)
    {
        logger.LogTrace("Updating book: {@book}", book);
        var originalBook = await bookRepository.GetBookByIdAsync(book.Isbn);

        if (originalBook == null)
        {
            throw new BookNotFoundException(book.Isbn);
        }
        
        var originalValues = new
        {
            Title = originalBook.Title,
            Description = originalBook.Description,
            PublishDate = originalBook.PublishDate,
            Authors = originalBook.Authors.ToList()
        };

        await bookRepository.UpdateBookAsync(book);

        var changes = new Dictionary<string, object>();
        if (originalValues.Title != book.Title)
            changes["Title"] = new { Old = originalValues.Title, New = book.Title };
        if (originalValues.Description != book.Description)
            changes["Description"] = new { Old = originalValues.Description, New = book.Description };
        if (originalValues.PublishDate != book.PublishDate)
            changes["PublishDate"] = new { Old = originalValues.PublishDate, New = book.PublishDate };
        if (!originalValues.Authors.SequenceEqual(book.Authors))
            changes["Authors"] = new { Old = originalValues.Authors, New = book.Authors };

        if (changes.Count != 0)
        {
            await auditService.LogChangeAsync(book.Isbn, "Updated", changes);
        }
    }
    public async Task DeleteBookAsync(string isbn)
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
}