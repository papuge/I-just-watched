using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Review
    {
        public int Id { get; set; }
        public User Author { get; set; } 
        [Display(Name = "Creation date time")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDateTime { get; set; }
        [Display(Name = "Review film")]
        public Film ReviewFilm { get; set; }
        public string Content { get; set; }
        // public HashSet<Tag> Tags { get; set; } // Int
        public ICollection<Comment> Comments { get; set; }
    }
}