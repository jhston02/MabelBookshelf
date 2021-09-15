using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record CreateNewBookshelfRequest
    {
        [Required] public Guid Id { get; init; }

        [Required] [MaxLength(50)] public string? Name { get; init; }
    }
}