using BookStore.DTOs;
using BookStore.Mappers;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories;

public class BookRepository : IBookRepository
{
    private readonly DbContext _context;
    

    public BookRepository(DbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        var books = await _context.Set<Book>()
            .AsNoTracking()
            .ToListAsync();

        return books.Select(BookMapper.ToDto);
    }

    public async Task<BookDto> GetBookByIdAsync(string isbn)
    {
        var book = await _context.Set<Book>().FindAsync(isbn);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with isbn {isbn} not found");
        }
        return BookMapper.ToDto(book);
    }

    public async Task AddBookAsync(BookDto bookDto)
    {
        var book = BookMapper.ToModel(bookDto);
        await _context.Set<Book>().AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(BookDto bookDto)
    {
        var book = BookMapper.ToModel(bookDto);
        var existingBook = await _context.Set<Book>().FindAsync(bookDto.Isbn);
        if (existingBook == null)
        {
            throw new KeyNotFoundException($"Book with id {bookDto.Isbn} not found");
        }
        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(string isbn)
    {
        var book = await _context.Set<Book>().FindAsync(isbn);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with id {isbn} not found");
        }
        _context.Set<Book>().Remove(book);
        await _context.SaveChangesAsync();
    }
}