using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Book.Commands
{
    public class CreateBookCommand : IRequest<bool>
    {
        [Required]
        [JsonInclude]
        public Guid Id { get; private set; }
        [Required]
        [JsonInclude]
        public string ExternalId { get; private set; }
        [Required]
        [JsonInclude]
        public string OwnerId { get; private set; }

        public CreateBookCommand(Guid id, string externalId, string ownerId)
        {
            this.Id = id;
            this.ExternalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            this.OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }
    }
}