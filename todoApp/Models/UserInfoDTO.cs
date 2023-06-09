using System;
using System.ComponentModel.DataAnnotations;

namespace todoApp.Models
{
    public class UserInfoDTO
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public int Phonenumber { get; set; }
    }
}

