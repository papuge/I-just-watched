namespace IJustWatched.Models
{
    public class SubscriptionJoin
    {
        public int Id { get; set; }
        public User CurrentUserId { get; set; }
        public User SubscribedOnId { get; set; }
        
        // TODO: 2 methods for getting subscribers and subscriptions
    }
}