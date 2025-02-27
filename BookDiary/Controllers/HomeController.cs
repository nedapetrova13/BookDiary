using System.Diagnostics;
using System.Linq.Expressions;
using BookDiary.Core.IServices;
using BookDiary.Models;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;


        public HomeController(ILogger<HomeController> logger,INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public async Task<IActionResult> LoggedIndex()
        {
           
            var newsList = await _newsService.GetTop5Services();

            var viewModelList = newsList.Select(news => new NewsCreateViewModel
            {
                Title = news.Title,
                Content = news.Content,
               
                
            }).ToList();

            return View(viewModelList);
        
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
