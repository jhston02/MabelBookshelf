namespace MabelBookshelf.Bookshelf.Infrastructure.BookshelfPreview
{
    public class BookshelfPreviewConfiguration
    {
        public string DatabaseName { get; set; }
        public int Version { get; set; } = 1;
        public string CollectionName { get; set; }
    }
}