using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkAsNotFinishedCommand : IRequest<bool>
    {
        public string BookId { get; private set; }

        public MarkAsNotFinishedCommand(string bookId)
            { 
                this.BookId = bookId;
            }
        }
}
