using BookStore.DTOs;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[Route("api/[controller]")]
[ApiController]

public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BookController> _logger;

    public BookController(IBookService bookService, ILogger<BookController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
    {
        _logger.Log(LogLevel.Information, "Getting all books");
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{isbn}")]
    public async Task<ActionResult<BookDto>> GetBookByIsbn(string isbn)
    {
        var book = await _bookService.GetBookByIdAsync(isbn);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook(BookDto bookDto)
    {
        var createdBook = await _bookService.AddBookAsync(bookDto);
        return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = createdBook.Isbn }, createdBook);
    }

    [HttpPut("{isbn}")]
    public async Task<IActionResult> UpdateBook(string isbn, BookDto bookDto)
    {
        if (isbn != bookDto.Isbn) return BadRequest();
        await _bookService.UpdateBookAsync(bookDto);
        return NoContent();
    }

    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBook(string isbn)
    {
        await _bookService.DeleteBookAsync(isbn);
        return NoContent();
    }
    
    
    
    


}