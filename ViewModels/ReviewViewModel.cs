using System.ComponentModel.DataAnnotations;
using IJustWatched.Models.CustomAttributes;

namespace IJustWatched.ViewModels
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "Title is required."), 
         MinLength(4, ErrorMessage = "Title is too short."), 
         MaxLength(150, ErrorMessage = "Title is too long.")]
        public string Title { get; set; }
        
        public string Tags { get; set; }
        
        [Required(ErrorMessage = "Film title is required."), 
         MinLength(1, ErrorMessage = "Title is too short."), 
         MaxLength(75, ErrorMessage = "Title is too long.")]
        [FilmInDbValidation]
        [Display(Name = "Film title")]
        public string FilmTitle { get; set; }
        
        [Required(ErrorMessage = "Content is required."),
         MinLength(5, ErrorMessage = "Post is too short."),
         MaxLength(4000, ErrorMessage = "Post is too long.")]
        [Display(Name = "Content")]
        public string ReviewText { get; set; }
    }
}