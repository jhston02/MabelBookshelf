using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class CreateNewBookshelfRequest
    {
        [Required] public Guid Id { get; set; }

        [Required] public string Name { get; set; }
    }
}