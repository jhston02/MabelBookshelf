using System;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class RemovedBookFromBookshelf : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public RemovedBookFromBookshelf(Guid bookShelfId, Guid bookId, long streamPosition) : base(bookShelfId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}