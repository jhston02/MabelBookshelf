namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class MarkedBookAsWantedDomainEvent : BookDomainEvent
    {
        public MarkedBookAsWantedDomainEvent(string bookId, long streamPosition) : base(bookId, streamPosition)
        {
        }
    }
}