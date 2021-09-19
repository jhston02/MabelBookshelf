using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record MarkAsWantedRequest
    {
        [Required] public string? Id { get; init; }
    }
}