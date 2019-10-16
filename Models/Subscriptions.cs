using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Models
{
    public class Subscriptions
    {
        public int Id { get; set; }
        public User SubscriberUser { get; set; }
        public User SubscriptionUser { get; set; }
        
        // TODO: check for correctness
        public async Task<ICollection<User>> GetUserSubscriptions(IJustWatchedContext context, string userId)
        {
            var users = from user in context.Subscriptions select user;
            var subscriptions = users.Where(user => user.SubscriberUser.Id == userId)
                .Select(user => user.SubscriptionUser);
            return await subscriptions.ToListAsync();
        }
        
        public async Task<ICollection<User>> GetUserSubscribers(IJustWatchedContext context, string userId)
        {
            var users = from user in context.Subscriptions select user;
            var subscriptions = users.Where(user => user.SubscriptionUser.Id == userId)
                .Select(user => user.SubscriberUser);
            return await subscriptions.ToListAsync();
        }
    }
}