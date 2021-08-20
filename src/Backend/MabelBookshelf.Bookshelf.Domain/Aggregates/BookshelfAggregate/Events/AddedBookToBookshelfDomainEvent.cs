using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookshelfAggregate.Events
{
    public class AddedBookToBookshelfDomainEvent : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public AddedBookToBookshelfDomainEvent(Guid streamId, Guid bookId, long streamPosition) : base(streamId, streamPosition)
        {
            this.BookId = bookId;
        }
    }
}