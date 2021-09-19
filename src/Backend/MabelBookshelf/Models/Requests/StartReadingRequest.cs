using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record StartReadingRequest
    {
        [Required] public string? Id { get; init; }
    }
}