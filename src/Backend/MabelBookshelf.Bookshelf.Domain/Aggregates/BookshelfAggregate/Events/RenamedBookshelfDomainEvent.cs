using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RenamedBookshelfDomainEvent : BookshelfDomainEvent
    {
        public RenamedBookshelfDomainEvent(Guid bookshelfId, string newName, string oldName, string ownerId) :
            base(bookshelfId, ownerId)
        {
            NewName = newName ?? throw new ArgumentNullException(nameof(newName));
            OldName = oldName ?? throw new ArgumentNullException(nameof(oldName));
        }

        public string NewName { get; }
        public string OldName { get; }
    }
}