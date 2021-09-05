namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class MarkedBookAsWantedDomainEvent : BookDomainEvent
    {
        public MarkedBookAsWantedDomainEvent(string bookId) : base(bookId)
        {
        }
    }
}