using System.Runtime.InteropServices;
using FastFood.Modals;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registermodel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registermodel.Email,
                    Email = registermodel.Email
                };
                //IdentityUser user = new IdentityUser
                //{
                //    Email = registermodel.Email,
                //    UserName = registermodel.Email
                //};
                var result = await _userManager.CreateAsync(user, registermodel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);  
                    return RedirectToAction("Login", "Account", new {Area="Identity"});
                }
                else
                {
                    foreach (var massage in result.Errors)
                    {
                        ModelState.AddModelError("", massage.Description);
                    }
                }
            }
            return View(registermodel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginmodel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginmodel.Email, loginmodel.Password, loginmodel.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Categories", new {Area="Admin"});
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
            }

            return View(loginmodel);
        }
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

