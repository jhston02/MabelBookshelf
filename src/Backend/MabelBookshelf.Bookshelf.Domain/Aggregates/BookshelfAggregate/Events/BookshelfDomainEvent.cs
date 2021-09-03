using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfDomainEvent : DomainEvent
    {
        public BookshelfDomainEvent(Guid bookshelfId, long streamPosition) : base(streamPosition)
        {
            BookshelfId = bookshelfId;
        }

        public Guid BookshelfId { get; }
    }
}