using System;
using System.ComponentModel.DataAnnotations;

namespace todoApp.Models
{
    public class LoginDTO
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}

