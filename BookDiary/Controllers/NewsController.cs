using BookDiary.Core.IServices;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _newsService.GetAllNews();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(News news)
        {
            await _newsService.Add(news);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var news = await _newsService.GetById(id);
            return View(news);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(News news)
        {
            if (ModelState.IsValid)
            {
                await _newsService.Update(news);
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
