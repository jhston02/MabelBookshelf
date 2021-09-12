using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record CreateNewBookRequest
    {
        [Required] public string? ExternalId { get; init; }
    }
}