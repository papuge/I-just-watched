using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using Microsoft.AspNetCore.Mvc;
using IJustWatched.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            var currentUser = GetCurrentUserAsync().Result;
            if (User.Identity.IsAuthenticated)
            {
                var subscriptions = _context.Subscriptions
                    .Where(sub => sub.SubscriberUser.Id == currentUser.Id)
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
        
        [Route("api/feed/{currentPage}")]
        public async Task<JsonResult> LoadReviewsPage(int currentPage)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Json("{}");
            }
            
            var isBackBtn = true;
            var isNextBtn = true;
            
            var currentUser = await GetCurrentUserAsync();
            var subscriptions = _context.Subscriptions
                .Where(sub => sub.SubscriberUser.Id == currentUser.Id)
                .Select(sub => sub.SubscriptionUser);

            var reviewsCount = _context.Reviews.Count(review => subscriptions.Contains(review.Author));

            int maxPage = (int)Math.Ceiling(reviewsCount / 5.0);
            if (currentPage > maxPage) currentPage = maxPage;
            if (currentPage < 1) currentPage = 1;
            
            if (maxPage == 1)
            {
                isBackBtn = false;
                isNextBtn = false;
            }
            else if (currentPage == maxPage)
                isNextBtn = false;
            else if (currentPage == 1)
                isBackBtn = false;

            var pageReviews = await _context.Reviews
                .Where(review => subscriptions.Contains(review.Author))  
                .OrderByDescending(r => r.CreationDateTime)
                .Skip((currentPage - 1) * 5)
                .Take(5)
                .Select(r => new
                {
                    id = r.Id,
                    authorId = r.Author.Id,
                    authorUserName = r.Author.UserName,
                    filmId = r.ReviewFilm.Id,
                    filmTitle = r.ReviewFilm.Title,
                    title = r.ReviewTitle,
                    date = r.CreationDateTime
                }).ToListAsync();

            return Json(JsonConvert.SerializeObject(new
            {
                isBackBtn,
                isNextBtn,
                currentPage,
                pageReviews
            }));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}