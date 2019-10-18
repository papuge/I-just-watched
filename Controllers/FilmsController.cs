using System.Linq;
using IJustWatched.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IJustWatched.Controllers
{
    public class FilmsController : Controller
    {
        private readonly IJustWatchedContext _context;
        public FilmsController(IJustWatchedContext context)
        {
            _context = context;
        }
        // GET
        public IActionResult Index()
        {
            return View(_context.Films.ToList());
        }
    }
}