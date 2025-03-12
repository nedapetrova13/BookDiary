using System.Diagnostics;
using System.Linq.Expressions;
using BookDiary.Core.IServices;
using BookDiary.Models;
using BookDiary.Models.ViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IUserService _userService;


        public HomeController(ILogger<HomeController> logger,INewsService newsService,IGenreService genreService,IAuthorService authorService,IBookService bookService,IUserService userService)
        {
            _logger = logger;
            _newsService = newsService;
            _authorService = authorService;
            _bookService = bookService;
            _genreService = genreService;
            _userService = userService;
        }

        public async Task<IActionResult> LoggedIndex()
        {
            var userlist = _userService.GetAll();
            List<UserIndexViewModel> users= new List<UserIndexViewModel>() ;
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


            var newsList = await _newsService.GetTop5Services();

            var viewModelList = newsList.Select(news => new NewsCreateViewModel
            {
                Title = news.Title,
                Content = news.Content,
               
                
            }).ToList();
            var authorscount = _authorService.GetAll().Count();
            var bookscount = _bookService.GetAll().Count();
            var genrecount = _genreService.GetAll().Count();
            var model = new IndexPageViewModel
            {
                News = viewModelList,
                BooksCount = bookscount,
                GenresCount = genrecount,
                AuthorsCount = authorscount,
                Users = users
            };
            return View(model);
        
        }
        public IActionResult Index()
        {
            return View();  
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
