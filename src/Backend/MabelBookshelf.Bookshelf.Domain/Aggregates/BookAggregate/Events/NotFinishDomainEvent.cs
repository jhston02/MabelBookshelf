using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class NotFinishDomainEvent : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public NotFinishDomainEvent(Guid bookId, long streamPosition) : base(bookId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}