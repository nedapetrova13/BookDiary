using BookDiary.Core.IServices;
using BookDiary.Models;
using BookDiary.Models.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Authorization;
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
                Id = news.Id 
            }).ToList();

            return View(viewModelList);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Add()
        {
            
            var model = new NewsCreateViewModel();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Add(NewsCreateViewModel ncvm)
        {
            if(ncvm.Title==null || ncvm.Content == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(ncvm);
            }
            else
            {
                var isExists = _newsService.Get(x=>x.Title == ncvm.Title);
                if(isExists == null)
                {
                    var news = new News
                    {
                        Title = ncvm.Title,
                        Content = ncvm.Content,
                    };
                    await _newsService.Add(news);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Вече съществува новина с такова име";
                    return View(ncvm);
                }
            }
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Edit(NewsEditViewModel nevm)
        {
            if(nevm.Title==null || nevm.Content == null)
            {
                TempData["error"] = "Невалидни данни";
                return View(nevm);
            }
            else
            {
                var isExists = _newsService.Get(x => x.Title == nevm.Title);
                if (isExists == null)
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
                else
                {
                    TempData["error"] = "Вече съществува новина с такова име";
                    return View(nevm);
                }
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
