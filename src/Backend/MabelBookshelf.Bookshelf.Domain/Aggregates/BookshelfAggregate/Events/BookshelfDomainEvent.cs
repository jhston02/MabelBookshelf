using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfDomainEvent : DomainEvent
    {
        public BookshelfDomainEvent(Guid bookshelfId)
        {
            BookshelfId = bookshelfId;
        }

        public Guid BookshelfId { get; }
    }
}