using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApp.Models
{
    public class UserDTO
    {
        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } 
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

