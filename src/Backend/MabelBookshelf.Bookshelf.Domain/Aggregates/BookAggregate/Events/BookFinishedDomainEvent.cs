using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookFinishedDomainEvent : DomainEvent
    {
        public BookFinishedDomainEvent(Guid streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}