using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record AddBookToBookshelfRequest
    {
        [Required] public string? BookId { get; init; }

        [Required] public Guid BookShelfId { get; init; }
    }
}