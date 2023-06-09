﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]

        public string PasswordHash { get; set; } = string.Empty;

        public string Address { get; set; }

        public int Phonenumber { get; set; }

    }
}

