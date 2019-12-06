using System.Collections.Generic;
using IJustWatched.Models;

namespace IJustWatched.ViewModels
{
    public class ProfileViewModel
    {
        public bool IsCurrentUserPage { get; set; }
        public User User { get; set; }
        public List<User> Followers { get; set; }
        public int FollowersCount { get; set; }
        public List<User> Following { get; set; }
        public int FollowingCount { get; set; }
        public List<Review> Reviews { get; set; }
    }
}