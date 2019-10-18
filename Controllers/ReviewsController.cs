using Microsoft.AspNetCore.Mvc;

namespace IJustWatched.Controllers
{
    public class ReviewsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}