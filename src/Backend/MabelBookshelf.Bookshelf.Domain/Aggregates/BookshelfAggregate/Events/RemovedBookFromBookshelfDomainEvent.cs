using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RemovedBookFromBookshelfDomainEvent : BookshelfDomainEvent
    {
        public RemovedBookFromBookshelfDomainEvent(Guid bookshelfId, string bookId, string ownerId) : base(
            bookshelfId, ownerId)
        {
            BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        }

        public string BookId { get; }
    }
}