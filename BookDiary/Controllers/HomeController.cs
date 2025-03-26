using System.Diagnostics;
using System.Linq.Expressions;
using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels;
using BookDiary.Models.ViewModels.BookViewModels;
using BookDiary.Models.ViewModels.CurrentReadViewModels;
using BookDiary.Models.ViewModels.NewsViewModels;
using BookDiary.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly UserManager<User> _userManager;
        private readonly ICurrentReadService _currentReadService;
        private readonly IShelfService _shelfService;
        private readonly IShelfBookService _shelfBookService;


        public HomeController(ILogger<HomeController> logger,UserManager<User> userManager, IShelfService shelfService, IShelfBookService shelfBookService, ICurrentReadService currentReadService, INewsService newsService,IGenreService genreService,IAuthorService authorService,IBookService bookService,IUserService userService)
        {
            _logger = logger;
            _newsService = newsService;
            _authorService = authorService;
            _bookService = bookService;
            _genreService = genreService;
            _userService = userService;
            _userManager = userManager;
            _currentReadService = currentReadService;
            _shelfService = shelfService;
            _shelfBookService = shelfBookService;
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

            var currentUser = await _userManager.GetUserAsync(User);
            var crs = await _currentReadService.Find(x => x.UserId == currentUser.Id);
            List<CurrentReadIndexViewModel> currentReads = new List<CurrentReadIndexViewModel>();
            foreach (var item in crs)
            {
                Book book = await _bookService.GetById(item.BookId);
                var currentread = new CurrentReadIndexViewModel
                {
                    Id = item.Id,
                    Userid = item.UserId,
                    BookId = item.BookId,
                    BookName = book.Title,
                    CoverImageURL = book.CoverImageURL,
                    Pages = item.CurrentPage,
                    TotalPages = book.BookPages
                };
                currentReads.Add(currentread);
            }
            List<BookSeriesViewModel> list = new List<BookSeriesViewModel>();

            var shelfid =await  _shelfService.Get(x => x.Name == "Прочетени книги");
            if(shelfid == null)
            {
               list = new List<BookSeriesViewModel>();
            }
            else
            {
                var shelf = await _shelfService.GetById(shelfid.Id);
                var books = _shelfService.GetAll()
                 .Where(b => b.Id == shelfid.Id)
                 .SelectMany(b => b.ShelfBooks.Select(x => x.Book))
                 .ToList();
                foreach (var b in books)
                {

                    var bookvm = new BookSeriesViewModel
                    {
                        Id = b.Id,
                        Title = b.Title,
                        CoverImageURL = b.CoverImageURL,
                    };
                    list.Add(bookvm);
                }

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
                Users = users,
                CurrentReads=currentReads,
                ReadBooks=list
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
