using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfCreatedDomainEvent : BookshelfDomainEvent
    {
        public BookshelfCreatedDomainEvent(Guid bookshelfId, string name, string ownerId, long streamPosition) : base(
            bookshelfId, streamPosition)
        {
            Name = name;
            OwnerId = ownerId;
        }

        public string Name { get; }
        public string OwnerId { get; }
    }
}