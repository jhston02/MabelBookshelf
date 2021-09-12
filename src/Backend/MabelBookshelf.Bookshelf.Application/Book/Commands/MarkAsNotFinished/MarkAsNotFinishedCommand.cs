using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public record MarkAsNotFinishedCommand : IRequest<bool>
    {
        public MarkAsNotFinishedCommand(string bookId)
        {
            BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        }

        public string BookId { get; }
    }
}