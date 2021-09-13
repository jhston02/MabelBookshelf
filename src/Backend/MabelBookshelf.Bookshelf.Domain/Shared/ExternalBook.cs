namespace MabelBookshelf.Bookshelf.Domain.Shared
{
    public record ExternalBook(string? Id, string? Title, string[]? Authors, string? Isbn, int TotalPages,
        string[]? Categories);
}