using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IJustWatched.Data;
using IJustWatched.Models;
using IJustWatched.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IJustWatched.Controllers
{
    public class UserController : Controller
    {
        private readonly IJustWatchedContext _context;
        private readonly UserManager<User> _userManager;
        private User _currentUser;
        private bool _isCurrentUser;
        private bool _isSubscription;

        public UserController(IJustWatchedContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index(string userId = "")
        {
            if (!HttpContext.User.Identity.IsAuthenticated && userId == "")
            {
                return RedirectToAction("Index", "Home");
            }
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                _currentUser = _context.Users.FirstOrDefault(u => u.Id == userId) as User;
                _isCurrentUser = false;
            }
            else
            {
                _currentUser = await GetCurrentUserAsync();
                if (userId == "" || userId == _currentUser.Id)
                {
                    _currentUser = await GetCurrentUserAsync();
                    _isCurrentUser = true;
                }
                else
                {
                    var thisUser = _currentUser;
                    _currentUser = _context.Users.FirstOrDefault(u => u.Id == userId) as User;
                    if (_currentUser != null)
                    {
                        var isSubs = _context.Subscriptions
                            .Where(subs => subs.SubscriberUser.Id == thisUser.Id)
                            .FirstOrDefault(s => s.SubscriptionUser.Id == _currentUser.Id);
                        _isSubscription = isSubs != null;
                        _isCurrentUser = false;
                    }
                }
            }

            var reviews = _context.Reviews
                .Where(review => review.Author.Id == _currentUser.Id)
                .Include(review => review.Author)
                .Include(review => review.ReviewFilm)
                .OrderByDescending(review => review.CreationDateTime)
                .ToListAsync();

            var viewModel = new ProfileViewModel
            {
                IsCurrentUserPage = _isCurrentUser,
                IsSubscription = _isSubscription,
                User = _currentUser,
                Reviews = reviews.Result
            };
            return View(viewModel);
        }

        [Route("api/reviews/{userId}/{currentPage}")]
        public async Task<JsonResult> LoadReviewsPage(string userId, int currentPage)
        {
            var isBackBtn = true;
            var isNextBtn = true;
            var reviewsCount = _context.Reviews.Count(r => r.Author.Id == userId);
            
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
                .Where(r => r.Author.Id == userId)  
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
        
        [Route("api/subscriptions/{userId}")]
        public async Task<JsonResult> LoadSubscriptions(string userId)
        {
            var followers = await _context.Subscriptions
                .Where(subs => subs.SubscriptionUser.Id == userId)
                .Include(subs => subs.SubscriberUser)
                .Select(subs => subs.SubscriberUser)
                .Select(f => new
                {
                    f.Id,
                    f.UserName,
                    f.PhotoUrl
                })
                .ToListAsync();
            
            var following =  await _context.Subscriptions
                .Where(subs => subs.SubscriberUser.Id == userId)
                .Include(subs => subs.SubscriptionUser)
                .Select(subs => subs.SubscriptionUser)
                .Select(f => new
                {
                    f.Id,
                    f.UserName,
                    f.PhotoUrl
                })
                .ToListAsync();
            return Json(JsonConvert.SerializeObject(new {followers, following}));
        }
        
        public async Task<IActionResult> Subscribe(string onUserId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (onUserId == currentUser.Id)
            {
                return Ok();
            }

            var existing = _context.Subscriptions
                .Where(subs => subs.SubscriberUser.Id == currentUser.Id)
                .FirstOrDefault(subs => subs.SubscriptionUser.Id == onUserId);
            if (existing is null)
            {
                _context.Subscriptions.Add(new Subscriptions
                {
                    SubscriberUser = currentUser,
                    SubscriptionUser = _context.Users.FindAsync(onUserId).Result as User
                });
                _context.SaveChanges();
            }
            return Ok();
        }
        
        public async Task<IActionResult> Unsubscribe(string onUserId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (onUserId == currentUser.Id)
            {
                return Ok();
            }
            var existing = _context.Subscriptions
                .Where(subs => subs.SubscriberUser.Id == currentUser.Id)
                .FirstOrDefault(subs => subs.SubscriptionUser.Id == onUserId);
            if (existing != null)
            {
                _context.Subscriptions.Remove(existing);
                _context.SaveChanges();
            }
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            _currentUser = GetCurrentUserAsync().Result;
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadedFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                _currentUser.PhotoUrl = "/images/" + fileName;
                _context.Update(_currentUser);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "User", new { userId = _currentUser.Id});
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}