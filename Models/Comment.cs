using System;
using System.ComponentModel.DataAnnotations;

namespace IJustWatched.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public User Author { get; set; }
        [Display(Name = "Creation Date Time")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDateTime { get; set; }
        public string Content { get; set; }
    }
}