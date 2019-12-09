using System;
using System.ComponentModel.DataAnnotations;
using IJustWatched.Models.CustomAttributes;

namespace IJustWatched.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "required")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "required")]
        public string Username { get; set; }
 
        [Required(ErrorMessage = "required")]
        [DataType(DataType.Date)]
        [BirthDateRange]
        [Display(Name = "birthday")]
        public DateTime BirthdayDate { get; set; }
 
        [Required(ErrorMessage = "required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Required(ErrorMessage = "required")]
        [Compare("Password", ErrorMessage = "passwNotMatch")]
        [DataType(DataType.Password)]
        [Display(Name = "passwRepeat")]
        public string PasswordConfirm { get; set; }

    }
}