using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Query.Queries.Preview.Models
{
    public record BookPreview(string BookId, string ExternalBookId, string[] Categories);
}