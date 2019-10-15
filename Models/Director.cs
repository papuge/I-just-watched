using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Director
    {
        public int Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Second name")]
        public string SecondName { get; set; }

        public override string ToString()
        {
            return $"{FirstName[0]}. {SecondName}";
        }
    }
}