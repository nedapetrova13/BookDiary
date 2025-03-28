using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.ProfileViewModels;
using BookDiary.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookDiary.Models.ViewModels.BookViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookDiary.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IBookService _bookService;

        public ProfileController(IUserService userService, UserManager<User> userManager, IBookService bookService)
        {
            _userService = userService;
            _userManager = userManager;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var userlist = _userService.GetAll();
            List<UserIndexViewModel> users = new List<UserIndexViewModel>();
            foreach (var item in userlist)
            {
                var user = new UserIndexViewModel
                {
                    Id = item.Id,
                    ProfilePictureURL = item.ProfilePictureURL,
                    Name = item.Name,
                };
                users.Add(user);
            }
            return View(users);    
        }
        public async Task<IActionResult> UserProfile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            
            
            var uservm = new UserProfileViewModel
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Bio = currentUser.Bio,
                Birthdate = currentUser.Birthdate,
            };
            if (currentUser.ProfilePictureURL != null)
            {
                uservm.ProfilePictureURL = currentUser.ProfilePictureURL;
            }
            
            return View(uservm);
        }
        public async Task<IActionResult> Edit()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var user = new EditProfileViewModel
            {
                Id = currentUser.Id,
                Bio = currentUser.Bio,
            };

            if (currentUser.ProfilePictureURL != null)
            {
                user.ProfilePictureURL = currentUser.ProfilePictureURL;
            }

            

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null)
            {
                return RedirectToAction("Index", "Home", null);
            }
            if (model.Bio != null)
            {
                currentUser.Bio = model.Bio;

            }
            else
            {
                TempData["error"] = "Невалидни данни";
                return View(model);
            }
            var result = await _userManager.UpdateAsync(currentUser);
            if (result.Succeeded)
            {
                return RedirectToAction("UserProfile");
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            bool isValidUser =  _userService.GetAll().Any(x=>x.Id==userId);

            if (!isValidUser)
            {
                TempData["error"] = "Този потребител не съществува.";
                return RedirectToAction("Index");
            }

            await _userService.DeleteUser(userId);

            return RedirectToAction("LoggedIndex","Home");
        }
    }
}
