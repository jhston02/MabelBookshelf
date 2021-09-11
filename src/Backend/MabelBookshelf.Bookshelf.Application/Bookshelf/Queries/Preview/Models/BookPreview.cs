using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models
{
    public class BookPreview
    {
        public string BookId { get; set; }
        public string ExternalBookId { get; set; }
        public List<string> Categories { get; set; }
    }
}