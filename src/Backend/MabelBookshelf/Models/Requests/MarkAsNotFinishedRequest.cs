using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class MarkAsNotFinishedRequest
    {
        [Required] public string BookId { get; set; }
    }
}