using System.Collections.Generic;
using IJustWatched.Models;

namespace IJustWatched.ViewModels
{
    public class ProfileViewModel
    {
        public User CurrentUser { get; set; }
        public List<User> Followers { get; set; }
        public int FollowersCount { get; set; }
        public List<User> Following { get; set; }
        public int FollowingCount { get; set; }
        public List<Review> Reviews { get; set; }
    }
}