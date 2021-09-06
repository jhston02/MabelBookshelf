using System.Collections.Generic;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models
{
    public class BookshelfPreview
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public List<string> Categories { get; set; }
        public List<BookPreview> Books { get; set; }
    }
}