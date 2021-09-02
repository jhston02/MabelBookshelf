using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookDeletedDomainEvent : DomainEvent
    {
        public BookDeletedDomainEvent(string streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}