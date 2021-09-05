namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookFinishedDomainEvent : BookDomainEvent
    {
        public BookFinishedDomainEvent(string bookId) : base(bookId)
        {
        }
    }
}