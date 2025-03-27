using BookDiary.Core.IServices;
using BookDiary.Models.ViewModels.Account;
using BookDiary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager
            ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Name, 
                Email = model.Email,
                Name = model.Name,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("LoggedIndex", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "Invalid email or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["success"] = "Успешно влязохте в профила си!";
                return RedirectToAction("LoggedIndex", "Home");
            }

            ModelState.AddModelError(nameof(model.Email), "Invalid email or password.");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
