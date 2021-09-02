using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookStartedDomainEvent : DomainEvent
    {
        public BookStartedDomainEvent(string streamId, long streamPosition) : base(streamId, streamPosition)
        {
        }
    }
}