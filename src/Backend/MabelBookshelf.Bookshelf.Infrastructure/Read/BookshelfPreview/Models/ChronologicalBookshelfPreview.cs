using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models;

namespace MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Models
{
    internal record ChronologicalBookshelfPreview(string Id, string Name, string OwnerId, List<BookPreview> Books) : Application.Bookshelf.Queries.Preview.Models.BookshelfPreview(Id, Name, OwnerId, Books)
    {
        public ulong StreamPosition { get; init; }
    }
}