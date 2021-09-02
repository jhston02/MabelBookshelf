using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class ReadToPageDomainEvent : DomainEvent
    {
        public int OldPageNumber { get; private set; }
        public int NewPageNumber { get; private set; }
        
        public ReadToPageDomainEvent(string streamId, int oldPageNumber, int newPageNumber, long streamPosition) : base(streamId, streamPosition)
        {
            this.OldPageNumber = oldPageNumber;
            this.NewPageNumber = newPageNumber;
        }
    }
}