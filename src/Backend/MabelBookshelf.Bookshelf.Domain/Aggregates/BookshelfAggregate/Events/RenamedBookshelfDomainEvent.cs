using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RenamedBookshelfDomainEvent : DomainEvent<Guid>
    {
        public string NewName { get; private set; }
        public string OldName { get; private set; }
        public RenamedBookshelfDomainEvent(Guid bookShelfId, string newName, string oldName, long streamPosition) : base(bookShelfId, streamPosition)
        {
            this.NewName = newName;
            this.OldName = oldName;
        }
    }
}