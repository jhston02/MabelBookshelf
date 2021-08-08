using System;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class ReadToPageDomainEvent : DomainEvent<Guid>
    {
        public Guid BookId { get; private set; }
        public int OldPageNumber { get; private set; }
        public int NewPageNumber { get; private set; }
        
        public ReadToPageDomainEvent(Guid bookId, int oldPageNumber, int newPageNumber, long streamPosition) : base(bookId, streamPosition)
        {
            this.BookId = bookId;
            this.OldPageNumber = oldPageNumber;
            this.NewPageNumber = newPageNumber;
        }
    }
}