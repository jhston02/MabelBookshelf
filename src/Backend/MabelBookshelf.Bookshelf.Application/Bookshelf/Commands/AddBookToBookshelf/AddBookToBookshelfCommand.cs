using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public record AddBookToBookshelfCommand : IRequest<bool>
    {
        public AddBookToBookshelfCommand(string? bookId, Guid shelfId, string ownerId)
        {
            BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
            ShelfId = shelfId;
            OwnerId = ownerId;
        }

        public string BookId { get; }
        public Guid ShelfId { get; }
        
        public string OwnerId { get; }
    }
}