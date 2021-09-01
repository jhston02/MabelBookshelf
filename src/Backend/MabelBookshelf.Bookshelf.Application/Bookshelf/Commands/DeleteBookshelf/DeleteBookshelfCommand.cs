using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class DeleteBookshelfCommand : IRequest<bool>
    {
        public Guid BookshelfId { get; private set; }
        public string OwnerId { get; private set; }

        public DeleteBookshelfCommand(Guid id, string ownerId)
        {
            this.BookshelfId = id;
            this.OwnerId = ownerId;
        }
    }
}