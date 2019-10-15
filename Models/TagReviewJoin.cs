namespace IJustWatched.Models
{
    public class TagReviewJoin
    {
        public int Id { get; set; }
        public Review TaggedReviewId { get; set; }
        public Tag PresentTagId { get; set; }
        
        // TODO: 2 methods for getting Tags and Reviews
    }
}