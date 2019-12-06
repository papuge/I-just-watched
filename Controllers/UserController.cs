using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using IJustWatched.Models;
using IJustWatched.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Controllers
{
    public class UserController : Controller
    {
        private readonly IJustWatchedContext _context;
        private readonly UserManager<User> _userManager;
        private User _currentUser;
        private bool _isCurrentUser;

        public UserController(IJustWatchedContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index(string userId = "")
        {
            if (userId == "")
            {
                _currentUser = await GetCurrentUserAsync();
                _isCurrentUser = true;
            }
            else
            {
                _currentUser = _context.Users.FirstOrDefault(u => u.Id == userId) as User;
                if (_currentUser != null)
                {
                    _isCurrentUser = false;
                }
            }

            var followers = _context.Subscriptions
                .Where(subs => subs.SubscriptionUser.Id == _currentUser.Id)
                .Include(subs => subs.SubscriberUser)
                .Select(subs => subs.SubscriberUser)
                .ToListAsync();
            
            var following = _context.Subscriptions
                .Where(subs => subs.SubscriberUser.Id == _currentUser.Id)
                .Include(subs => subs.SubscriptionUser)
                .Select(subs => subs.SubscriptionUser)
                .ToListAsync();

            var reviews = _context.Reviews
                .Where(review => review.Author.Id == _currentUser.Id)
                .Include(review => review.Author)
                .Include(review => review.ReviewFilm)
                .OrderByDescending(review => review.CreationDateTime)
                .ToListAsync();

            var viewModel = new ProfileViewModel
            {
                IsCurrentUserPage = _isCurrentUser,
                User = _currentUser,
                Followers = followers.Result,
                FollowersCount = followers.Result.Count,
                Following = following.Result,
                FollowingCount = following.Result.Count,
                Reviews = reviews.Result
            };
            return View(viewModel);
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}