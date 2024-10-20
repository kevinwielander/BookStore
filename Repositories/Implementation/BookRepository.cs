using BookStore.Data;
using BookStore.DTOs;
using BookStore.Exceptions;
using BookStore.Mappers;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories.Implementation;

public class BookRepository(BookStoreContext context, ILogger<BookRepository> logger) : IBookRepository
{
    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        logger.LogTrace("Retrieving books");
        var books = await context.Set<Book>()
            .AsNoTracking()
            .ToListAsync();

        return books.Select(BookMapper.ToDto);
    }

    public async Task<BookDto> GetBookByIdAsync(string isbn)
    {
        logger.LogTrace("Retrieving book with isbn {isbn}",isbn);
        var book = await context.Set<Book>().FindAsync(isbn);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with isbn {isbn} not found");
        }
        return BookMapper.ToDto(book);
    }

    public async Task AddBookAsync(BookDto bookDto)
    {
        var existingBook = await context.Set<Book>().FindAsync(bookDto.Isbn);
        if (existingBook != null)
        {
            throw new BookAlreadyExistsException(bookDto.Isbn);
        }
        var book = BookMapper.ToModel(bookDto);
        await context.Set<Book>().AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(BookDto bookDto)
    {
        var book = BookMapper.ToModel(bookDto);
        var existingBook = await context.Set<Book>().FindAsync(bookDto.Isbn);
        if (existingBook == null)
        {
            throw new KeyNotFoundException($"Book with id {bookDto.Isbn} not found");
        }
        var updatedBook = BookMapper.ToModel(bookDto);
        context.Entry(existingBook).CurrentValues.SetValues(updatedBook);
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