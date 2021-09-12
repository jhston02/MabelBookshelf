using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public record MarkAsNotFinishedRequest
    {
        [Required] public string BookId { get; init; }
    }
}