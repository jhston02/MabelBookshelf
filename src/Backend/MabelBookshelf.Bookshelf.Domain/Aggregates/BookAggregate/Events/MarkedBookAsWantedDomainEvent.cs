using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class MarkedBookAsWantedDomainEvent : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public MarkedBookAsWantedDomainEvent(Guid bookId, long streamPosition) : base(bookId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}