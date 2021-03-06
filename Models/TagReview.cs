using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Models
{
    public class TagReview
    {
        public int Id { get; set; }
        public Review TaggedReview { get; set; }
        public Tag TagInReview { get; set; }

        public async Task<ICollection<Review>> GetReviewsFromTag(IJustWatchedContext context,
            int tagId)
        {
            var rows = from row in context.TagsReviews select row;
            var reviews = rows.Where(row => row.TagInReview.Id == tagId)
                .Select(user => user.TaggedReview);
            return await reviews.ToListAsync();
        }
    }
}