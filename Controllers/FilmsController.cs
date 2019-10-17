using Microsoft.AspNetCore.Mvc;

namespace IJustWatched.Controllers
{
    public class FilmsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}