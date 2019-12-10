using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IJustWatched.Data;
using Microsoft.AspNetCore.Mvc;
using IJustWatched.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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
            return View();
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
            
            if (maxPage == 1 || maxPage == 0)
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
        
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response?.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
 
            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Search(string query)
        {
            if (query != null)
            {
                var words = Regex.Split(query, @"\s+")
                    .Where(word => word != "")
                    .Select(word => word.Trim('#').ToLower());
                var tags = _context.Tags.Where(tag => words.Contains(tag.TagText));
                var reviewsByTag = await _context.TagsReviews
                    .Include(tr => tr.TagInReview)
                    .Include(tr => tr.TaggedReview)
                    .Where(tr => tags.Contains(tr.TagInReview))
                    .Select(tr => tr.TaggedReview)
                    .Include(r => r.Author)
                    .Include(r => r.ReviewFilm)
                    .ToListAsync();
                
                var reviewsByFilm = await _context.Reviews
                    .Include(r => r.Author)
                    .Include(r => r.ReviewFilm)
                    .Where(r => words.Contains(r.ReviewFilm.Title, StringComparer.OrdinalIgnoreCase))
                    .ToListAsync();

                var result = reviewsByTag.Union(reviewsByFilm).ToList();
                return View(result);
            }

            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}