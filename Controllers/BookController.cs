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
        try
        {
            _logger.LogInformation("Retrieving all books");
            var books = await _bookService.GetAllBooksAsync();
            _logger.LogInformation("Retrieved {Count} books", books.Count);
            return Ok(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all books");
            return StatusCode(500, "An error occurred while retrieving books");
        }
    }

    [HttpGet("{isbn}")]
    public async Task<ActionResult<BookDto>> GetBookByIsbn(string isbn)
    {
        try
        {
            _logger.LogInformation("Retrieving book with ISBN {Isbn}", isbn);
            var book = await _bookService.GetBookByIdAsync(isbn);
            if (book == null)
            {
                _logger.LogWarning("Book with ISBN {Isbn} not found", isbn);
                return NotFound($"Book with ISBN {isbn} not found");
            }
            return Ok(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving book with ISBN {Isbn}", isbn);
            return StatusCode(500, $"An error occurred while retrieving the book with ISBN {isbn}");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook(BookDto bookDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid book data submitted: {@BookDto}", bookDto);
            return BadRequest(ModelState);
        }
        try
        {
            _logger.LogInformation("Adding new book {Book}", bookDto);
            var createdBook = await _bookService.AddBookAsync(bookDto);
            _logger.LogInformation("Book added successfully with ISBN {Isbn}", createdBook.Isbn);
            return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = createdBook.Isbn }, createdBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding new book");
            return StatusCode(500, "An error occurred while adding the book");
        }
    }

    [HttpPut("{isbn}")]
    public async Task<IActionResult> UpdateBook(string isbn, BookDto bookDto)
    {
        if (isbn != bookDto.Isbn)
        {
            _logger.LogWarning("Mismatch between route ISBN {RouteIsbn} and body ISBN {BodyIsbn}", isbn, bookDto.Isbn);
            return BadRequest("ISBN in the route does not match the ISBN in the book data");
        }

        try
        {
            _logger.LogInformation("Updating book with ISBN {Isbn}", isbn);
            await _bookService.UpdateBookAsync(bookDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Book with ISBN {Isbn} not found for update", isbn);
            return NotFound($"Book with ISBN {isbn} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating book with ISBN {Isbn}", isbn);
            return StatusCode(500, $"An error occurred while updating the book with ISBN {isbn}");
        }
    }

    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBook(string isbn)
    {
        try
        {
            _logger.LogInformation("Deleting book with ISBN {Isbn}", isbn);
            await _bookService.DeleteBookAsync(isbn);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Book with ISBN {Isbn} not found for deletion", isbn);
            return NotFound($"Book with ISBN {isbn} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting book with ISBN {Isbn}", isbn);
            return StatusCode(500, $"An error occurred while deleting the book with ISBN {isbn}");
        }
    }
}
