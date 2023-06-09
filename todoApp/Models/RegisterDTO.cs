using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace todoApp.Models
{
    public class RegisterDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Address { get; set; }

        public int Phonenumber { get; set; }

    }
}

