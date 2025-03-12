using BookDiary.Core.IServices;
using BookDiary.Models;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace BookDiary.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        public IActionResult Index()
        {
            var newsList = _newsService.GetAll();

            var viewModelList = newsList.Select(news => new NewsCreateViewModel
            {
                Title = news.Title,
                Content = news.Content,
                Id = news.Id // Ensure Id is mapped for Edit/Delete actions
            }).ToList();

            return View(viewModelList);
        }
        public IActionResult Add()
        {
            
            var model = new NewsCreateViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(NewsCreateViewModel ncvm)
        {
            var news = new News
            {
                Title = ncvm.Title,
                Content = ncvm.Content,
            };
            await _newsService.Add(news);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsService.GetById(id);
            if (news == null)
            {
                return NotFound(); 
            }
            var model = new NewsEditViewModel
            {
                Title = news.Title,
                Content = news.Content
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsEditViewModel nevm)
        {
            var model = new News
            {
                Id = nevm.Id,
                Title = nevm.Title,
                Content = nevm.Content,
            };
            await _newsService.Update(model);
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
