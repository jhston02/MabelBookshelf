using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public record AddBookToBookshelfCommand : IRequest<bool>
    {
        public AddBookToBookshelfCommand(string bookId, Guid shelfId)
        {
            BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
            ShelfId = shelfId;
        }

        public string BookId { get; }
        public Guid ShelfId { get; }
    }
}