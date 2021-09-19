using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public record StartReadingCommand : IRequest<bool>
    {
        public StartReadingCommand(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }
    }
}