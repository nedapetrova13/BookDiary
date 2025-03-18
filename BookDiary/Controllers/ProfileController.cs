using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.ProfileViewModels;
using BookDiary.Models.ViewModels.UserViewModels;
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
            var userlist = userService.GetAll();
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
        
    }
}
