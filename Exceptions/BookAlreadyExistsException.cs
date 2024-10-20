namespace BookStore.Exceptions;

public class BookAlreadyExistsException(string isbn) : Exception($"A book with ISBN {isbn} already exists")
{
    public string Isbn { get; } = isbn;
}