using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public record DeleteBookshelfCommand : IRequest<bool>
    {
        public DeleteBookshelfCommand(Guid id, string ownerId)
        {
            Id = id;
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }

        public Guid Id { get; }
        public string OwnerId { get; }
    }
}