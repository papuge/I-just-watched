using System.Linq;
using IJustWatched.Data;
using Microsoft.AspNetCore.Mvc;

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
            return View(film);
        }
    }
}