using System;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookAggregate.Events
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