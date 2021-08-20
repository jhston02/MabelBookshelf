using System;
using System.ComponentModel.DataAnnotations;

namespace MabelBookshelf.Models
{
    public class CreateNewBookRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ExternalId { get; set; }
    }
}