using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class CreateBookCommand : IRequest<bool>
    {
        public string ExternalId { get; private set; }
        public string OwnerId { get; private set; }

        public CreateBookCommand(string externalId, string ownerId)
        {
            this.ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            this.OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }
    }
}