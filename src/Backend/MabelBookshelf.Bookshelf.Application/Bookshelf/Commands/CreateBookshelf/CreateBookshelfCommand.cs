using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public record CreateBookshelfCommand : IRequest<bool>
    {
        public CreateBookshelfCommand(Guid id, string? name, string ownerId)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }

        public Guid Id { get; }
        public string Name { get; }
        public string OwnerId { get; }
    }
}