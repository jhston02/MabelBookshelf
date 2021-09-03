using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelfDomainEvent : BookshelfDomainEvent
    {
        public AddedBookToBookshelfDomainEvent(Guid bookshelfId, string bookId) : base(bookshelfId)
        {
            BookId = bookId;
        }

        public string BookId { get; }
    }
}