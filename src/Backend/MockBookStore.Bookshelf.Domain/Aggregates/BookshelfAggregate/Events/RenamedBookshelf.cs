using System;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RenamedBookshelf : DomainEvent<Guid>
    {
        public string NewName { get; private set; }
        public string OldName { get; private set; }
        public RenamedBookshelf(Guid bookShelfId, string newName, string oldName, long streamPosition) : base(bookShelfId, streamPosition)
        {
            this.NewName = newName;
            this.OldName = oldName;
        }
    }
}