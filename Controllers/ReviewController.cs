using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IJustWatched.Data;
using IJustWatched.Models;
using IJustWatched.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IJustWatchedContext _context;
        private readonly UserManager<User> _userManager;

        public ReviewController(IJustWatchedContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET
        public IActionResult Index(int reviewId)
        {
            var review = _context.Reviews.Where(item => item.Id == reviewId)
                                         .Include(r => r.Author)
                                         .Include(r => r.ReviewFilm)
                                         .First();
            return View(review);
        }
        // GET
        public IActionResult New(string filmTitle = null)
        {
            ViewData["filmTitle"] = filmTitle;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ReviewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review
                {
                    Author = await GetCurrentUserAsync(),
                    Content = viewModel.ReviewText,
                    CreationDateTime = DateTime.Now,
                    ReviewTitle = viewModel.Title,
                    ReviewFilm = _context.Films.First(film => film.Title.Equals(viewModel.FilmTitle, 
                        StringComparison.OrdinalIgnoreCase))
                };
                _context.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index", "Review", new { reviewId = review.Id});
            }
            return View(viewModel);
        }
        
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        
    }
}