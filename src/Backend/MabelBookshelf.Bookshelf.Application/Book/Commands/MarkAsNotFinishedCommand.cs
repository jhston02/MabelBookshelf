using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkAsNotFinishedCommand : IRequest<bool>
    {
        public MarkAsNotFinishedCommand(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; }
    }
}