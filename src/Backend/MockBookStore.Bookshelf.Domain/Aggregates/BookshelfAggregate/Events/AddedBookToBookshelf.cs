using System;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelf : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public AddedBookToBookshelf(Guid bookShelfId, Guid bookId, long streamPosition) : base(bookShelfId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}