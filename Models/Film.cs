using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public ICollection<Director> DirectorsId { get; set; }
    }
}