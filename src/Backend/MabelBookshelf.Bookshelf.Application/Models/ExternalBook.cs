namespace MabelBookshelf.Bookshelf.Application.Models
{
    public class ExternalBook
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string[] Authors { get; private set; }
        public string Isbn { get; private set; }
        public int TotalPages { get; private set; }
        public string[] Categories { get; private set; }
        
        public ExternalBook(string id, string title, string[] authors, string isbn, int totalPages, string[] categories)
        {
            this.Id = id;
            this.Title = title;
            this.Authors = authors;
            this.Isbn = isbn;
            this.TotalPages = totalPages;
            this.Categories = categories;
        }
    }
}