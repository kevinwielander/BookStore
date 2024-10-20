using System.Data;
using BookStore.DTOs;
using BookStore.Exceptions;
using BookStore.Mappers;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[Route("api/v1/books")]
[ApiController]

public class BookController(IBookService bookService, ILogger<BookController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
    {
        try
        {
            logger.LogInformation("Retrieving all books");
            var books = await bookService.GetAllBooksAsync();
            logger.LogInformation("Retrieved {Count} books", books.Count);
            var bookDtos = books.Select(BookMapper.ToDto).ToList();
            return Ok(bookDtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all books");
            return StatusCode(500, "An error occurred while retrieving books");
        }
    }

    [HttpGet("{isbn}")]
    public async Task<ActionResult<BookDto>> GetBookByIsbn(string isbn)
    {
        try
        {
            logger.LogInformation("Retrieving book with ISBN {Isbn}", isbn);
            var book = await bookService.GetBookByIdAsync(isbn);
            var bookDto = BookMapper.ToDto(book);
            return Ok(bookDto);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Book with ISBN {Isbn} not found", isbn);
            return NotFound($"Book with ISBN {isbn} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving book with ISBN {Isbn}", isbn);
            return StatusCode(500, $"An error occurred while retrieving the book with ISBN {isbn}");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook(BookDto bookDto)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid book data submitted: {@BookDto}", bookDto);
            return BadRequest(ModelState);
        }
        try
        {
            logger.LogInformation("Adding new book {bookDto}", bookDto);
            var book = BookMapper.ToModel(bookDto);
            var createdBook = await bookService.AddBookAsync(book);
            logger.LogInformation("Book added successfully with ISBN {Isbn}", createdBook.Isbn);
            return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = createdBook.Isbn }, createdBook);
        }
        catch (BookAlreadyExistsException ex)
        {
            logger.LogWarning(ex, "Attempt to add a book with existing ISBN {Isbn}", ex.Isbn);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding new book");
            return StatusCode(500, "An error occurred while adding the book");
        }
    }

    [HttpPut("{isbn}")]
    public async Task<IActionResult> UpdateBook(string isbn, BookDto bookDto)
    {
        if (isbn != bookDto.Isbn)
        {
            logger.LogWarning("Mismatch between route ISBN {RouteIsbn} and body ISBN {BodyIsbn}", isbn, bookDto.Isbn);
            return BadRequest("ISBN in the route does not match the ISBN in the book data");
        }

        try
        {
            logger.LogInformation("Updating book with ISBN {Isbn}", isbn);
            var book = BookMapper.ToModel(bookDto);
            await bookService.UpdateBookAsync(book);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Book with ISBN {Isbn} not found for update", isbn);
            return NotFound($"Book with ISBN {isbn} not found");
        }
        catch (DBConcurrencyException ex)
        {
            logger.LogError(ex, "Concurrency conflict detected for book with ISBN {Isbn}", isbn);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating book with ISBN {Isbn}", isbn);
            return StatusCode(500, $"An error occurred while updating the book with ISBN {isbn}");
        }
    }

    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBook(string isbn)
    {
        try
        {
            logger.LogInformation("Deleting book with ISBN {Isbn}", isbn);
            await bookService.DeleteBookAsync(isbn);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Book with ISBN {Isbn} not found for deletion", isbn);
            return NotFound($"Book with ISBN {isbn} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting book with ISBN {Isbn}", isbn);
            return StatusCode(500, $"An error occurred while deleting the book with ISBN {isbn}");
        }
    }
}
