using System.ComponentModel.DataAnnotations;
using IJustWatched.Models.CustomAttributes;

namespace IJustWatched.ViewModels
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "required"), 
         MinLength(4, ErrorMessage = "tooShort"), 
         MaxLength(150, ErrorMessage = "tooLong")]
        [Display(Name = "title")]
        public string Title { get; set; }
        
        [Display(Name = "tags")]
        public string Tags { get; set; }
        
        [Required(ErrorMessage = "required"), 
         MinLength(1, ErrorMessage = "tooShort"), 
         MaxLength(75, ErrorMessage = "tooLong")]
        [FilmInDbValidation]
        [Display(Name = "filmTitle")]
        public string FilmTitle { get; set; }
        
        [Required(ErrorMessage = "required"),
         MinLength(5, ErrorMessage = "tooShort"),
         MaxLength(4000, ErrorMessage = "tooLong")]
        [Display(Name = "content")]
        public string ReviewText { get; set; }
    }
}