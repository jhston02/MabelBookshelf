using System;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate.Events
{
    public class BookCreatedDomainEvent : BookDomainEvent
    {
        public BookCreatedDomainEvent(string bookId, string title, string[] authors, string isbn, string externalId,
            int totalPages, string ownerId, string[] categories) : base(bookId)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Authors = authors ?? throw new ArgumentNullException(nameof(authors));
            Isbn = isbn ?? throw new ArgumentNullException(nameof(isbn));
            ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            TotalPages = totalPages;
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
            Categories = categories ?? throw new ArgumentNullException(nameof(categories));
        }

        public string Title { get; }
        public string[] Authors { get; }
        public string Isbn { get; }
        public string ExternalId { get; }
        public int TotalPages { get; }
        public string OwnerId { get; }
        public string[] Categories { get; }
    }
}