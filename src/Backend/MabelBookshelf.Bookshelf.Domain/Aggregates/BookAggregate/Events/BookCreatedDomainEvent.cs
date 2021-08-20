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
        public string OwnerId { get; private set; }
        public string[] Categories { get; private set; }

        public BookCreatedDomainEvent(Guid streamId, string title, string[] authors, string isbn, string externalId, int totalPages, long streamPosition, string ownerId, string[] categories) : base(streamId, streamPosition)
        {
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.Authors = authors ?? throw new ArgumentNullException(nameof(authors));
            this.Isbn = isbn ?? throw new ArgumentNullException(nameof(isbn));
            this.ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            this.TotalPages = totalPages;
            this.OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
            this.Categories = categories ?? throw new ArgumentNullException(nameof(categories));
        }
    }
}