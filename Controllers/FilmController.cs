using System.Linq;
using IJustWatched.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Controllers
{
    public class FilmController : Controller
    {
        private readonly IJustWatchedContext _context;

        public FilmController(IJustWatchedContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Index(int filmId)
        {
            var film = _context.Films.First(flm => flm.Id == filmId);
            var reviews = _context.Reviews
                .Where(review => review.ReviewFilm == film)
                .Include(r => r.Author)
                .ToList();
            ViewData["Reviews"] = reviews;
            return View(film);
        }
    }
}