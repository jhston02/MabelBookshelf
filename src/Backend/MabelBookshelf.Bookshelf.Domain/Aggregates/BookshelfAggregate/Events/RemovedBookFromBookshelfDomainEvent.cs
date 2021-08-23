using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RemovedBookFromBookshelfDomainEvent : DomainEvent
    {
        public Guid BookId { get; private set; }
        public RemovedBookFromBookshelfDomainEvent(Guid streamId, Guid bookId, long streamPosition) : base(streamId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}