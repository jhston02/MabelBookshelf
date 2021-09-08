using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class ReadToPageCommand : IRequest<bool>
    {
        public string BookId { get; private set; }
        public int PageNumber { get; private set; }

        public ReadToPageCommand(string bookId, int pageNumber)
        {
            this.BookId = bookId;
            this.PageNumber = pageNumber;
        }
    }
}