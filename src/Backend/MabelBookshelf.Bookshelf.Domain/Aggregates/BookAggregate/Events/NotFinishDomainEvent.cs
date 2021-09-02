using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class NotFinishDomainEvent : DomainEvent
    {
        public NotFinishDomainEvent(string streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}