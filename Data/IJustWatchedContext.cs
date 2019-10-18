using IJustWatched.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Data
{
    public class IJustWatchedContext: IdentityDbContext
    {
        public IJustWatchedContext(DbContextOptions<IJustWatchedContext> 
            options) : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagReview> TagsReviews { get; set; }
    }
}