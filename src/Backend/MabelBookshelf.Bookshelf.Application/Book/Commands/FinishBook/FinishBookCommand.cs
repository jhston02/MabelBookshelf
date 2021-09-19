using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public record FinishBookCommand : IRequest<bool>
    {
        public FinishBookCommand(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }
    }
}