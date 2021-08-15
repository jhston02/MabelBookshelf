using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MabelBookshelf.Bookshelf.Application.Bookshelf.Commands
{
    public class CreateBookshelfCommand : IRequest<bool>
    {
        [Required]
        public Guid Id { get; private set; }
        [Required]
        public string Name { get; private set; }
        [Required]
        public long OwnerId { get; private set; }

        public CreateBookshelfCommand(Guid id, string name, long ownerId)
        {
            this.Id = id;
            this.Name = name;
            this.OwnerId = ownerId;
        }
    }
}