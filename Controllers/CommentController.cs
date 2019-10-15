using Microsoft.AspNetCore.Mvc;

namespace IJustWatched.Controllers
{
    public class CommentController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}