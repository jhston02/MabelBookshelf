using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfCreatedDomainEvent : BookshelfDomainEvent
    {
        public BookshelfCreatedDomainEvent(Guid bookshelfId, string name, string ownerId) : base(
            bookshelfId, ownerId)
        {
            if (name != null) Name = name;
        }

        public string Name { get; }
    }
}