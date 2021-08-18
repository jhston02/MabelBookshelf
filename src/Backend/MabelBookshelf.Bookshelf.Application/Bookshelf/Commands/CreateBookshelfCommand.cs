using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class CreateBookshelfCommand : IRequest<bool>
    {
        [Required]
        [JsonInclude]
        public Guid Id { get; private set; }
        [Required]
        [JsonInclude]
        public string Name { get; private set; }
        [Required]
        [JsonInclude]
        public string OwnerId { get; private set; }

        public CreateBookshelfCommand(Guid id, string name, string ownerId)
        {
            this.Id = id;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        }
    }
}