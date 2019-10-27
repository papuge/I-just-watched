using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Models
{
    public class LikeReview
    {
        public int Id { get; set; }
        public Review LikedReview { get; set; }
        public User LikedByUser { get; set; }
        
        // TODO: check for correctness
        public async Task<ICollection<User>> GetReviewLikes(IJustWatchedContext context,
            int reviewId)
        {
            var rows = from row in context.LikesReviews select row;
            var likes = rows.Where(row => row.LikedReview.Id == reviewId)
                .Select(row => row.LikedByUser);
            return await likes.ToListAsync();
        }
        
        public async Task<ICollection<Review>> GetLikedReviews(IJustWatchedContext context,
            string userId)
        {
            var rows = from row in context.LikesReviews select row;
            var likedReviews = rows.Where(row => row.LikedByUser.Id == userId)
                .Select(row => row.LikedReview);
            return await likedReviews.ToListAsync();
        }
    }
}