namespace MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview.Models
{
    internal class ChronologicalBookshelfPreview : Application.Bookshelf.Queries.Preview.Models.BookshelfPreview
    {
        public ulong StreamPosition { get; set; }
    }
}