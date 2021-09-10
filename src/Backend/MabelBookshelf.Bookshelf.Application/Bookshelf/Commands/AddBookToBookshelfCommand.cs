using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class AddBookToBookshelfCommand : IRequest<bool>
    {
        public AddBookToBookshelfCommand(string bookId, Guid shelfId)
        {
            BookId = bookId;
            ShelfId = shelfId;
        }

        public string BookId { get; }
        public Guid ShelfId { get; }
    }
}