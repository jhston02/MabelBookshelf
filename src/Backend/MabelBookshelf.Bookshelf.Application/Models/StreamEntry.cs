using System.Reflection.Metadata;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Application.Models
{
    public class StreamEntry
    {
        public DomainEvent DomainEvent { get; }
        public ulong StreamPosition { get; }

        public StreamEntry(ulong streamPosition, DomainEvent domainEvent)
        {
            this.StreamPosition = streamPosition;
            this.DomainEvent = domainEvent;
        }
    }
}