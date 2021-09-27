using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Query.Models
{
    public record StreamEntry(ulong StreamPosition, DomainEvent DomainEvent);
}