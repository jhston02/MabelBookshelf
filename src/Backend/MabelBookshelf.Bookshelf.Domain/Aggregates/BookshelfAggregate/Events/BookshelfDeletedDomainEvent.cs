using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfDeletedDomainEvent : DomainEvent
    {
        public BookshelfDeletedDomainEvent(Guid streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}