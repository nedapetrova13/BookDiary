using BookDiary.Core.IServices;
using BookDiary.Core.Services;
using BookDiary.Models;
using BookDiary.Models.ViewModels.GenreViewModels;
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

        public IActionResult Index()
        {
            var list =  _genreService.GetAll();
            return View(list);
        }

        public IActionResult Add()
        {
            var model = new GenreCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GenreCreateViewModel gcvm)
        {
            var genre = new Genre
            {
                Name = gcvm.Name
            };
            await _genreService.Add(genre);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _genreService.GetById(id);
            var model = new GenreEditViewModel
            {
                Name = tag.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreEditViewModel gevm) 
        {
            var model = new Genre
            {
                Id = gevm.Id,
                Name = gevm.Name
            };
            await _genreService.Update(model);
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
