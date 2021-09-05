using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RemovedBookFromBookshelfDomainEvent : BookshelfDomainEvent
    {
        public RemovedBookFromBookshelfDomainEvent(Guid bookshelfId, string bookId) : base(
            bookshelfId)
        {
            BookId = bookId;
        }

        public string BookId { get; }
    }
}