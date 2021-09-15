using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public record DeleteBookshelfCommand : IRequest<bool>
    {
        public DeleteBookshelfCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}