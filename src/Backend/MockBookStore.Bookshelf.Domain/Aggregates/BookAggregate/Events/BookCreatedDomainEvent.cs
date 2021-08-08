using System;
using MockBookStore.Bookshelf.Domain.SeedWork;

namespace MockBookStore.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookCreatedDomainEvent : DomainEvent<Guid>
    {
        public  Book Book { get; private set; }
        public BookCreatedDomainEvent(Book book) : base(book.Id, book.Version)
        {
            Book = book;
        }
    }
}