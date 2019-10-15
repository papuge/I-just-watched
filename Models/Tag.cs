using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(20)]
        public string TagText { get; set; }

        public override string ToString()
        {
            return $"{TagText.ToLower()}";
        }
    }
}