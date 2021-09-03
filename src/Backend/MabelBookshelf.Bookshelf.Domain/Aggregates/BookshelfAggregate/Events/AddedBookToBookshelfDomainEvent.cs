using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelfDomainEvent : BookshelfDomainEvent
    {
        public AddedBookToBookshelfDomainEvent(Guid bookshelfId, string bookId, long streamPosition) : base(bookshelfId,
            streamPosition)
        {
            BookId = bookId;
        }

        public string BookId { get; }
    }
}