using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class MarkAsWantedRequest
    {
        [Required] public string? Id { get; init; }
    }
}