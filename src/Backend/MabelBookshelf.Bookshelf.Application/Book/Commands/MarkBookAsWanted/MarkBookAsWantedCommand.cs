using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class MarkBookAsWantedCommand : IRequest<bool>
    {
        public MarkBookAsWantedCommand(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }
    }
}