using System;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public User Author { get; set; }
        public Review CommentedReview { get; set; }
        [Display(Name = "Creation Date Time")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDateTime { get; set; }
        [Required, MaxLength(500)]
        public string Content { get; set; }
    }
}