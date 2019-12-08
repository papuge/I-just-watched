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
            else if (!HttpContext.User.Identity.IsAuthenticated)
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
                    _currentUser = _context.Users.FirstOrDefault(u => u.Id == userId) as User;
                    if (_currentUser != null)
                    {
                        _isCurrentUser = false;
                    }
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