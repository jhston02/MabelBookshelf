using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class FinishRequest
    {
        [Required] public string? Id { get; init; }
    }
}