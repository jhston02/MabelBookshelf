using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfCreatedDomainEvent : DomainEvent<Guid>
    {
        public string Name { get; private set; }
        public string OwnerId { get; private set; }
        
        public BookshelfCreatedDomainEvent(Guid id, string name, string ownerId, long streamPosition) : base(id, streamPosition)
        {
            this.Name = name;
            this.OwnerId = ownerId;
        }
    }
}