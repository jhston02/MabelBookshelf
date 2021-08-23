using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class NotFinishDomainEvent : DomainEvent
    {
        public NotFinishDomainEvent(Guid streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}