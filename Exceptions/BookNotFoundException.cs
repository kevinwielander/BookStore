namespace BookStore.Exceptions;

public class BookNotFoundException(string isbn) : Exception($"A book with ISBN {isbn} does not exists")
{
    public string Isbn { get; } = isbn;
}