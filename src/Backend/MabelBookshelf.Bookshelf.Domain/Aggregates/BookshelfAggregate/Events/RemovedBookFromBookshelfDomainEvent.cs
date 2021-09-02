using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RemovedBookFromBookshelfDomainEvent : DomainEvent
    {
        public string BookId { get; private set; }
        public RemovedBookFromBookshelfDomainEvent(string streamId, string bookId, long streamPosition) : base(streamId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}