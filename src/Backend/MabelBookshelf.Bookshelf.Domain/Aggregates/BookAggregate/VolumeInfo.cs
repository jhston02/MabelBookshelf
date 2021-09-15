using System;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Domain.Shared;

namespace MabelBookshelf.Bookshelf.Domain.Aggregates.BookAggregate
{
    public record VolumeInfo
    {
        internal VolumeInfo(string? title, string[]? authors, string? isbn, string? externalId, int totalPages,
            string[]? categories)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Authors = authors ?? throw new ArgumentNullException(nameof(authors));
            Isbn = isbn ?? throw new ArgumentNullException(nameof(isbn));
            ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            TotalPages = totalPages;
        }

        public string Title { get; }
        public string[] Authors { get; }
        public string Isbn { get; }
        public string ExternalId { get; }
        public int TotalPages { get; }
        public string[] Categories { get; }

        public static async Task<VolumeInfo> FromExternalId(string externalId, IExternalBookService service,
            CancellationToken cancellationToken = default)
        {
            var externalBook = await service.GetBookAsync(externalId, cancellationToken);
            
            return new VolumeInfo(externalBook.Id, externalBook.Authors, externalBook.Isbn, externalId,
                externalBook.TotalPages, externalBook.Categories);
        }
    }
}