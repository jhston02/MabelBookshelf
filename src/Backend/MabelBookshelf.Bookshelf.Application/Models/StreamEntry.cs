using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Models
{
    public class StreamEntry
    {
        public StreamEntry(ulong streamPosition, DomainEvent domainEvent)
        {
            StreamPosition = streamPosition;
            DomainEvent = domainEvent;
        }

        public DomainEvent DomainEvent { get; }
        public ulong StreamPosition { get; }
    }
}