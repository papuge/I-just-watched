using System.ComponentModel.DataAnnotations;

namespace IJustWatched.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="emailReq")]
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage="passwReq")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         
        [Display(Name = "rememberMe")]
        public bool RememberMe { get; set; }
         
        public string ReturnUrl { get; set; }
    }
}