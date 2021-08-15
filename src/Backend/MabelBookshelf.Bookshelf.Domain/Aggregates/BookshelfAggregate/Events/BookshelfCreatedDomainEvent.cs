using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class BookshelfCreatedDomainEvent : DomainEvent<Guid>
    {
        public string Name { get; private set; }
        public long OwnerId { get; private set; }
        
        public BookshelfCreatedDomainEvent(Guid id, string name, long ownerId, long streamPosition) : base(id, streamPosition)
        {
            this.Name = name;
            this.OwnerId = ownerId;
        }
    }
}