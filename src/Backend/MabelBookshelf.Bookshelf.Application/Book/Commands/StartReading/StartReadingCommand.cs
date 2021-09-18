using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands.StartReading
{
    public record StartReadingCommand : IRequest<bool>
    {
        public string Id { get; }

        public StartReadingCommand(string id)
        {
            this.Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}