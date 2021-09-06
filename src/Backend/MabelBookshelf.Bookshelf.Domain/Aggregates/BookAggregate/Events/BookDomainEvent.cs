using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookDomainEvent : DomainEvent
    {
        public BookDomainEvent(string bookId, string ownerId)
        {
            BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }

        public string BookId { get; }
        public string OwnerId { get; }
    }
}