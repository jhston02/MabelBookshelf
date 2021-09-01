using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class DeleteBookshelfRequest
    {
        [Required]
        public Guid BookshelfId { get; set; }
    }
}