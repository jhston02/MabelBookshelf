namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class ReadToPageDomainEvent : BookDomainEvent
    {
        public ReadToPageDomainEvent(string bookId, int oldPageNumber, int newPageNumber, string ownerId) : base(
            bookId, ownerId)
        {
            OldPageNumber = oldPageNumber;
            NewPageNumber = newPageNumber;
        }

        public int OldPageNumber { get; }
        public int NewPageNumber { get; }
    }
}