using System;
using System.ComponentModel.DataAnnotations;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book
{
    public class CreateBookCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; private set; }
        [Required]
        public string ExternalId { get; private set; }
        [Required]
        public string OwnerId { get; private set; }

        public CreateBookCommand(Guid id, string externalId, string ownerId)
        {
            this.Id = id;
            this.ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            this.OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }
    }
}