using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class DeleteBookshelfCommand : IRequest<bool>
    {
        public DeleteBookshelfCommand(Guid id, string ownerId)
        {
            Id = id;
            OwnerId = ownerId;
        }

        public Guid Id { get; }
        public string OwnerId { get; }
    }
}