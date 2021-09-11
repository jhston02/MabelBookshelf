using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfDomainEvent : DomainEvent
    {
        public BookshelfDomainEvent(Guid bookshelfId, string ownerId)
        {
            BookshelfId = bookshelfId;
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }

        public Guid BookshelfId { get; }
        public string OwnerId { get; }
    }
}