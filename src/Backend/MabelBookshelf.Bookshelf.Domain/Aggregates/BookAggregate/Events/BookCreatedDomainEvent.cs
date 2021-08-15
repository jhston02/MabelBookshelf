using System;
using MabelBookshelf.Bookshelf.Domain.SeedWork;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookCreatedDomainEvent : DomainEvent<Guid>
    {
        public string Title { get; private set; }
        public string[] Authors { get; private set; }
        public string Isbn { get; private set; }
        public string ExternalId { get; private set; }
        public int TotalPages { get; private set; }

        public BookCreatedDomainEvent(Guid id, string title, string[] authors, string isbn, string externalId, int totalPages, long streamPosition) : base(id, streamPosition)
        {
            this.Title = title;
            this.Authors = authors;
            this.Isbn = isbn;
            this.ExternalId = externalId;
            this.TotalPages = totalPages;
        }
    }
}