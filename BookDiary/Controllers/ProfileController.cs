using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.ProfileViewModels;
using BookDiary.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookDiary.Models.ViewModels.BookViewModels;

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
            var favbookid = currentUser.FavouriteBookId;
            
            var uservm = new UserProfileViewModel
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                ProfilePictureURL = currentUser.ProfilePictureURL,
                Bio = currentUser.Bio,
            };
            return View();
        }
    }
}
