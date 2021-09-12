using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Infrastructure.Models
{
    public record StreamEntry(ulong StreamPosition, DomainEvent DomainEvent);
}