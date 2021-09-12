namespace MabelBookshelf.Bookshelf.Application.Models
{
    public record ExternalBook(string? Id, string? Title, string[]? Authors, string? Isbn, int TotalPages,
        string[]? Categories);
}