using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RenamedBookshelfDomainEvent : BookshelfDomainEvent
    {
        public RenamedBookshelfDomainEvent(Guid bookshelfId, string newName, string oldName, long streamPosition) :
            base(bookshelfId, streamPosition)
        {
            NewName = newName;
            OldName = oldName;
        }

        public string NewName { get; }
        public string OldName { get; }
    }
}