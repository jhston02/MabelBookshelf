using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class CreateNewBookRequest
    {
        [Required]
        public string ExternalId { get; set; }
    }
}