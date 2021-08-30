using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class MarkedBookAsWantedDomainEvent : DomainEvent
    {
        public MarkedBookAsWantedDomainEvent(Guid streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}