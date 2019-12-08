using System.Collections.Generic;
using System.Threading.Tasks;
using IJustWatched.Models;
using IJustWatched.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IJustWatched.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
 
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        // GET
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = viewModel.Email, UserName = viewModel.Username, 
                    BirthdayDate = viewModel.BirthdayDate, PhotoUrl = "images/missing_avatar.png"};
                // get result of user create operation
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    // set up cookies
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRolesAsync(user, new List<string> {"user"});
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(viewModel);
        }
        
        // GET
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.EmailOrUsername);
                Microsoft.AspNetCore.Identity.SignInResult result;
                if (user != null)
                {
                    // Username was entered at login form
                    result = await _signInManager.PasswordSignInAsync(user.UserName, 
                        viewModel.Password, viewModel.RememberMe, false);
                }
                else
                {
                    // Username was entered at login form
                    result = await _signInManager.PasswordSignInAsync(viewModel.EmailOrUsername,
                        viewModel.Password, viewModel.RememberMe, false);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }
            return View(viewModel);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logoff()
        {
            // delete auth cookies
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}