using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class DeleteBookshelfRequest
    {
        [Required] public Guid Id { get; set; }
    }
}