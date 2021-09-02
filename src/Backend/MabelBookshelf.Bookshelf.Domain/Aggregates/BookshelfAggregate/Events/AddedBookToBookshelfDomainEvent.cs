using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelfDomainEvent : DomainEvent
    {
        public string BookId { get; private set; }
        public AddedBookToBookshelfDomainEvent(string streamId, string bookId, long streamPosition) : base(streamId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}