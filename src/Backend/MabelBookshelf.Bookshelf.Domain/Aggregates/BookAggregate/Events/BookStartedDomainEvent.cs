namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookStartedDomainEvent : BookDomainEvent
    {
        public BookStartedDomainEvent(string bookId) : base(bookId)
        {
        }
    }
}