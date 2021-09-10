using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelfDomainEvent : BookshelfDomainEvent
    {
        public AddedBookToBookshelfDomainEvent(Guid bookshelfId, string bookId, string ownerId) : base(bookshelfId,
            ownerId)
        {
            BookId = bookId;
        }

        public string BookId { get; }
    }
}