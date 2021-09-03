using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfCreatedDomainEvent : BookshelfDomainEvent
    {
        public BookshelfCreatedDomainEvent(Guid bookshelfId, string name, string ownerId) : base(
            bookshelfId)
        {
            Name = name;
            OwnerId = ownerId;
        }

        public string Name { get; }
        public string OwnerId { get; }
    }
}