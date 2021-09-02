using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookFinishedDomainEvent : DomainEvent
    {
        public BookFinishedDomainEvent(string streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}