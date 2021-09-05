using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RenamedBookshelfDomainEvent : BookshelfDomainEvent
    {
        public RenamedBookshelfDomainEvent(Guid bookshelfId, string newName, string oldName) :
            base(bookshelfId)
        {
            NewName = newName;
            OldName = oldName;
        }

        public string NewName { get; }
        public string OldName { get; }
    }
}