namespace MabelBookshelf.Bookshelf.Infrastructure.Models
{
    public record ProjectionPosition(ulong CommitPosition, ulong PreparePosition);
}