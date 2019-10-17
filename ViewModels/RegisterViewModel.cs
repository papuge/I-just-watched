using System;
using System.ComponentModel.DataAnnotations;
using IJustWatched.Models.CustomAttributes;

namespace IJustWatched.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Field doesn't math email address.'")]
        public string Email { get; set; }
 
        [Required]
        [DataType(DataType.Date)]
        [BirthDateRange]
        [Display(Name = "Birthday Date")]
        public DateTime BirthdayDate { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        public string PasswordConfirm { get; set; }

    }
}