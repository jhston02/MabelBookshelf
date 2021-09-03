using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookDomainEvent : DomainEvent
    {
        public BookDomainEvent(string bookId, long streamPosition) : base(streamPosition)
        {
            BookId = bookId;
        }

        public string BookId { get; }
    }
}