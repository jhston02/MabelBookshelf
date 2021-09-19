using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record FinishRequest
    {
        [Required] public string? Id { get; init; }
    }
}