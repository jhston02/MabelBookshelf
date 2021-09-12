using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models
{
    public record BookPreview(string BookId, string ExternalBookId, List<string> Categories);
}