using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using Microsoft.AspNetCore.Mvc;
using IJustWatched.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJustWatchedContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(IJustWatchedContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Review> feedReviews = null;
            var currentUser = GetCurrentUserAsync();
            if (User.Identity.IsAuthenticated)
            {
                var subscriptions = _context.Subscriptions
                    .Where(sub => sub.SubscriberUser.Id == currentUser.Result.Id)
                    .Select(sub => sub.SubscriptionUser);

                feedReviews = _context.Reviews
                    .Where(review => subscriptions.Contains(review.Author))
                    .Include(review => review.Author)
                    .Include(review => review.ReviewFilm)
                    .Take(6)
                    .OrderByDescending(review => review.CreationDateTime)
                    .ToList();
            }
            return View(feedReviews);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}