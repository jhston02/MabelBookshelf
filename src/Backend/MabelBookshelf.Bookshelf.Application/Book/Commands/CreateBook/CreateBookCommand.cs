using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public record CreateBookCommand : IRequest<string>
    {
        public CreateBookCommand(string? externalId, string ownerId)
        {
            ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }

        public string ExternalId { get; }
        public string OwnerId { get; }
    }
}