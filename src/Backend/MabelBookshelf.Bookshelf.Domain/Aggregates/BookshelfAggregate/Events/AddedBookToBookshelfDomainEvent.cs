using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelfDomainEvent : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public AddedBookToBookshelfDomainEvent(Guid bookShelfId, Guid bookId, long streamPosition) : base(bookShelfId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}