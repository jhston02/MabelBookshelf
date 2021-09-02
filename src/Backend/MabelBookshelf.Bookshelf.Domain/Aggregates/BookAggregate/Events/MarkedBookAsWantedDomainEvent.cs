using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class MarkedBookAsWantedDomainEvent : DomainEvent
    {
        public MarkedBookAsWantedDomainEvent(string streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}