using System;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class CreateBookshelfCommand : IRequest<bool>
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string OwnerId { get; private set; }

        public CreateBookshelfCommand(string id, string name, string ownerId)
        {
            this.Id = id;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }
    }
}