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
    public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
    {
        _logger.Log(LogLevel.Information, "Getting all books");
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDTO>> GetBookById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook(BookDTO bookDto)
    {
        var createdBook = await _bookService.AddBookAsync(bookDto);
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, BookDTO bookDto)
    {
        if (id != bookDto.Id) return BadRequest();
        await _bookService.UpdateBookAsync(bookDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteBookAsync(id);
        return NoContent();
    }
    
    
    
    


}