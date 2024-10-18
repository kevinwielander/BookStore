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


    public async Task<IEnumerable<BookDTO>> GetBooksAsync()
    {
        var books = await _context.Set<Book>()
            .AsNoTracking()
            .ToListAsync();

        return books.Select(BookMapper.ToDto);
    }

    public async Task<BookDTO> GetBookByIdAsync(int id)
    {
        var book = await _context.Set<Book>().FindAsync(id);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with id {id} not found");
        }
        return BookMapper.ToDto(book);
    }

    public async Task AddBookAsync(BookDTO bookDto)
    {
        var book = BookMapper.ToModel(bookDto);
        await _context.Set<Book>().AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(BookDTO bookDto)
    {
        var book = BookMapper.ToModel(bookDto);
        var existingBook = await _context.Set<Book>().FindAsync(bookDto.Id);
        if (existingBook == null)
        {
            throw new KeyNotFoundException($"Book with id {bookDto.Id} not found");
        }
        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Set<Book>().FindAsync(id);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with id {id} not found");
        }
        _context.Set<Book>().Remove(book);
        await _context.SaveChangesAsync();
    }
}