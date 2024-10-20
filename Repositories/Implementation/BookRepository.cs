using BookStore.Data;
using BookStore.DTOs;
using BookStore.Exceptions;
using BookStore.Mappers;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories.Implementation;

public class BookRepository(BookStoreContext context, ILogger<BookRepository> logger) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        logger.LogTrace("Retrieving books");
        var books = await context.Set<Book>()
            .AsNoTracking()
            .ToListAsync();

        return books;
    }

    public async Task<Book> GetBookByIdAsync(string isbn)
    {
        logger.LogTrace("Retrieving book with isbn {isbn}",isbn);
        var book = await context.Set<Book>().FindAsync(isbn);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with isbn {isbn} not found");
        }

        return book;
    }

    public async Task AddBookAsync(Book book)
    {
        var existingBook = await context.Set<Book>().FindAsync(book.Isbn);
        if (existingBook != null)
        {
            throw new BookAlreadyExistsException(book.Isbn);
        }
        await context.Set<Book>().AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        var existingBook = await context.Set<Book>().FindAsync(book.Isbn);
        if (existingBook == null)
        {
            throw new KeyNotFoundException($"Book with id {book.Isbn} not found");
        }
        context.Entry(existingBook).CurrentValues.SetValues(book);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(string isbn)
    {
        var book = await context.Set<Book>().FindAsync(isbn);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with id {isbn} not found");
        }
        context.Set<Book>().Remove(book);
        await context.SaveChangesAsync();
    }
}