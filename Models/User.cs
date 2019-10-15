using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(25)]
        public string Username { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }
    }
}