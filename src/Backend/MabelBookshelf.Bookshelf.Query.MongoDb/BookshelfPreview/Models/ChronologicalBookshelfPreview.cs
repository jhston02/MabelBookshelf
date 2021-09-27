using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Query.Queries.Preview.Models;

namespace MabelBookshelf.Bookshelf.Query.MongoDb.Models
{
    internal record ChronologicalBookshelfPreview
        (string Id, string Name, string OwnerId, List<BookPreview> Books) :
            BookshelfPreview(Id, Name, OwnerId, Books)
    {
        public ulong StreamPosition { get; init; }
    }
}