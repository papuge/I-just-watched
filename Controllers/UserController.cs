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

        public UserController(IJustWatchedContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index()
        {
            _currentUser = await GetCurrentUserAsync();
            
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

            var viewModel = new ProfileViewModel
            {
                CurrentUser = _currentUser,
                Followers = followers.Result,
                FollowersCount = followers.Result.Count,
                Following = following.Result,
                FollowingCount = following.Result.Count
            };
            return View(viewModel);
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}