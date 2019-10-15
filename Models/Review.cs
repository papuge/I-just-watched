using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public User Author { get; set; }
        
        [Display(Name = "Creation Date Time")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDateTime { get; set; }
        
        [Display(Name = "Review film")]
        [Required]
        public Film ReviewFilm { get; set; }
        
        [Required, MinLength(4), MaxLength(150)]
        public string ReviewTitle { get; set; }
        
        [Required, MinLength(5), MaxLength(4000)]
        public string Content { get; set; }
        [Required]
        public ICollection<Comment> Comments { get; set; }
    }
}