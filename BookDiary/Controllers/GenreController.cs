using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookDiary.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _genreService.GetAllGenres();
            return View(list);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Genre genre)
        {
            await _genreService.Add(genre);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _genreService.GetById(id);
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre genre) 
        {
            await _genreService.Update(genre);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
