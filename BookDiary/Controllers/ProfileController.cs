using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService userService;

        public ProfileController(IUserService _userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AdminProfile(string userId)
        {
            var user = await userService.Get(x=>x.Id==userId);
            if (user==null)
            {
                return RedirectToAction("LoggedIndex", "Home");
            }

            var model = new AdminProfileViewModel
            {
                Id = userId,
                Name = user.Name,
                ProfilePictureUrl = user.ProfilePictureURL,
                Email = user.Email
            };

            return View("Profile",model);
        }
    }
}
