using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record DeleteBookshelfRequest
    {
        [Required] public Guid Id { get; init; }
    }
}