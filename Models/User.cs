using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IJustWatched.Models.CustomAttributes;
using Microsoft.AspNetCore.Identity;

namespace IJustWatched.Models
{
    public class User: IdentityUser
    {
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }
        public string PhotoUrl { get; set; }
    }
}