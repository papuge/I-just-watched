using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Director
    {
        public int Id { get; set; }
        [Required, MaxLength(40)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required, MaxLength(40)]
        [Display(Name = "Second name")]
        public string SecondName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {SecondName}";
        }
    }
}