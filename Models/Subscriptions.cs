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
    }
}