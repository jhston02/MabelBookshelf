using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class ReadToPageRequest
    {
        [Required]
        public string BookId { get; set; }

        [Required] 
        public int PageNumber { get; set; }
    }
}