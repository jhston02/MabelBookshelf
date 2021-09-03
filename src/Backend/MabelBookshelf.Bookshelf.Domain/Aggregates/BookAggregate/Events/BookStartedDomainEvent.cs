namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookStartedDomainEvent : BookDomainEvent
    {
        public BookStartedDomainEvent(string bookId, long streamPosition) : base(bookId, streamPosition)
        {
        }
    }
}