using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class AddBookToBookshelfRequest
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public Guid BookShelfId { get; set; }  
    }
}