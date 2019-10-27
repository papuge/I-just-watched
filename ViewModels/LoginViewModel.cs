using System.ComponentModel.DataAnnotations;

namespace IJustWatched.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string EmailOrUsername { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
         
        public string ReturnUrl { get; set; }
    }
}