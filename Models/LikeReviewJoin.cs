namespace IJustWatched.Models
{
    public class LikeReviewJoin
    {
        public int Id { get; set; }
        public Review LikedReviewId { get; set; }
        public User LikedByUserId { get; set; }
        
        // TODO: 2 methods for getting Likes and Liked Posts
    }
}