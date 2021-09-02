using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfDeletedDomainEvent : DomainEvent
    {
        public BookshelfDeletedDomainEvent(string streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}