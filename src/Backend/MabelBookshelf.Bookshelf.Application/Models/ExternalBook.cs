namespace MabelBookshelf.Bookshelf.Application.Models
{
    public class ExternalBook
    {
        public ExternalBook(string id, string title, string[] authors, string isbn, int totalPages, string[] categories)
        {
            Id = id;
            Title = title;
            Authors = authors;
            Isbn = isbn;
            TotalPages = totalPages;
            Categories = categories;
        }

        public string Id { get; }
        public string Title { get; }
        public string[] Authors { get; }
        public string Isbn { get; }
        public int TotalPages { get; }
        public string[] Categories { get; }
    }
}