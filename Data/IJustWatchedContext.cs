using IJustWatched.Models;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Data
{
    public class IJustWatchedContext: DbContext
    {
        public IJustWatchedContext(DbContextOptions<IJustWatchedContext> 
            options) : base(options)
        {
            
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionJoin> Subscriptions { get; set; }
        public DbSet<TagReviewJoin> TagsReviews { get; set; }
        public DbSet<LikeReviewJoin> LikesReviews { get; set; }
    }
}