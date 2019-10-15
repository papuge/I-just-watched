using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Film
    {
        public int Id { get; set; }
        [Required, MinLength(1), MaxLength(75)]
        public string Title { get; set; }

        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public ICollection<Director> Directors { get; set; }
    }
}