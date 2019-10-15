using Microsoft.AspNetCore.Mvc;

namespace IJustWatched.Controllers
{
    public class UserController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}