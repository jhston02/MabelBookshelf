using System.Collections.Generic;
using MabelBookshelf.Bookshelf.Query.Queries.Preview.Models;

namespace MabelBookshelf.Bookshelf.Query.MongoDb.BookshelfUpcaster.Models
{
    public record BookUpcastInfo(string Id, string Title, string[] Authors, string ExternalId,
        string OwnerId, string[] Categories, string Status, ulong StreamPosition);
}