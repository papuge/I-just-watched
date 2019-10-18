using System;
using System.Collections.Generic;
using IJustWatched.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IJustWatched.Controllers
{
    public class ReviewController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
        // actions
        public IActionResult Show()
        {
            var review = new Review()
            {
                CreationDateTime = DateTime.Now,
                Comments = new List<Comment>(),
                Content = "Some badass stuff, but cool",
                ReviewFilm = new Film() { Title = "Hateful Eight"}
            };
            return View(review);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New()
        {
            return View();
        }
    }
}